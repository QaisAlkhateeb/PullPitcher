// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using PullPitcher.Pulls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using static System.Collections.Specialized.BitVector32;

namespace PullPitcher.Dialogs
{
    public class MainDialog : ComponentDialog, ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly IPitcherAppService _pitcherAppService;
        private readonly ICatcherAppService _catchersAppService;
        private readonly IPullRequestAppService _pullPitchesTracking;

        // TODO: Move to settings
        private Regex _pullRequestRegex = new Regex(@"https:\/\/dev\.azure\.com\/([^\/]+)\/([^\/]+)\/_git\/([^\/]+)\/pullrequest\/(\d+)");
        private Dictionary<string, WaterfallStep> CommandsHandlerMap;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(ILogger<MainDialog> logger, IPitcherAppService pitcherAppService,
            ICatcherAppService catchersAppService, IPullRequestAppService pullPitchesTracking)
            : base(nameof(MainDialog))
        {
            _logger = logger;
            _pitcherAppService = pitcherAppService;
            _catchersAppService = catchersAppService;
            _pullPitchesTracking = pullPitchesTracking;

            var waterfallSteps = new WaterfallStep[]
            {
                    HandleCommandAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> HandleCommandAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            try
            {
                stepContext.Context.Activity.Text = stepContext.Context.Activity.Text.Replace("<at>PullPitcherBot</at>", "").Trim();
                var text = stepContext.Context.Activity.Text.Trim();

                if (text.StartsWith("SetCatchers"))
                {
                    return await HandleSetCatchersCommand(stepContext, cancellationToken);
                }

                if (text.StartsWith("ListCatchers"))
                {
                    return await HandleListCatchersCommand(stepContext, cancellationToken);
                }

                if (text.StartsWith("Pitch"))
                {
                    return await HandlePitchCommandAsync(stepContext, cancellationToken);
                }
                
                if (text.StartsWith("History"))
                {
                    return await HandleHistoryCommand(stepContext, cancellationToken);
                }

                if (text.StartsWith("Me"))
                {
                    return await HandleMeCommand(stepContext, cancellationToken);
                }

                if (text.StartsWith("To"))
                {
                    return await HandleToCommand(stepContext, cancellationToken);
                }
                await stepContext.Context.SendActivityAsync($"Command Not Found!");

                return await stepContext.EndDialogAsync();
            }
            catch (BusinessException ex)
            {
                await stepContext.Context.SendActivityAsync(ex.Message);
                return await stepContext.EndDialogAsync();
            }
           
        }

        private async Task<DialogTurnResult> HandleSetCatchersCommand(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var parts = stepContext.Context.Activity.Text.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string repoKey = parts[1].Trim();
                string catcherEmails = parts.Length > 3 ? parts[3].Trim() : "";

                var members = await TeamsInfo.GetPagedMembersAsync(stepContext.Context);
                List<SetCatcherDto> newCatchers;

                if (string.IsNullOrEmpty(catcherEmails))
                {
                    newCatchers = members.Members.Select(member => new SetCatcherDto
                    {
                        ExternalId = member.Id,
                        Name = member.Name,
                        Email = member.Email
                    }).ToList();
                }
                else
                {
                    var emailList = catcherEmails.Split(',').Select(email => email.Trim()).ToList();
                    newCatchers = members.Members.Where(member => emailList.Contains(member.Email)).Select(member => new SetCatcherDto
                    {
                        ExternalId = member.Id,
                        Name = member.Name,
                        Email = member.Email
                    }).ToList();
                }

                await _catchersAppService.SetCatchers(repoKey, newCatchers);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Catchers set successfully for " + repoKey), cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Usage: SetCatchers \"repoKey\" \"email1,email2,...\""), cancellationToken);
            }
            

            return await stepContext.EndDialogAsync();
        }

        private async Task<DialogTurnResult> HandlePitchCommandAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Parse Pull Request Link
            var message = stepContext.Context.Activity.Text;
            Match match = _pullRequestRegex.Match(message);

            if (!match.Success && stepContext.Context.Activity.ChannelId == "msteams" && stepContext.Context.Activity.Attachments != null && stepContext.Context.Activity.Attachments.Count > 0)
            {
                message = stepContext.Context.Activity.Attachments[0].Content.ToString();
                match = _pullRequestRegex.Match(message);
            }

            // Pitch Request If Success
            if (match.Success)
            {
                var OwnerId = stepContext.Context.Activity.From.Id;
                // TODO Get Link only
                var pullReviewers = await _pitcherAppService.Pitch(match.Value, OwnerId);

                //var mentions = new List<Entity>();
                //foreach (var pullReviewer in pullReviewers)
                //{
                //    var catcher = pullReviewer.Catcher;
                //    var mention = new Mention
                //    {
                //        // Mentioned = turnContext.Activity.From,
                //        Mentioned = new ChannelAccount(catcher.ExternalId, catcher.Name),
                //        Text = $"<at>{XmlConvert.EncodeName(catcher.Name)}</at>",
                //    };

                //    mentions.Add(mention);
                //}

                // Consider First Reviewer as Main Reviewer 
                var mainReviewer = pullReviewers.First().Catcher;
                var mention = new Mention
                {
                    // Mentioned = turnContext.Activity.From,
                    Mentioned = new ChannelAccount(mainReviewer.ExternalId, mainReviewer.Name),
                    Text = $"<at>{XmlConvert.EncodeName(mainReviewer.Name)}</at>",
                };
                var replyActivity = MessageFactory.Text($"Pull {match.Value} Review Assigned To {mention.Text}");
                replyActivity.Entities = new List<Entity> { mention };

                await stepContext.Context.SendActivityAsync(replyActivity, cancellationToken);
            }


            return await stepContext.EndDialogAsync();
        }

        public async Task<DialogTurnResult> HandleListCatchersCommand(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var parts = stepContext.Context.Activity.Text.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                var repoKey = parts[1];
                List<CatcherListItemDto> catchers = await _catchersAppService.ListCatchers(repoKey);

                if (catchers != null && catchers.Count > 0)
                {
                    string messageText = "Catchers for repo '" + repoKey + "':<br>" + string.Join("<br>", catchers.Select(c => $"{c.Name} ({c.Email})"));
                    await stepContext.Context.SendActivityAsync(messageText, cancellationToken: cancellationToken);
                }
                else
                {
                    await stepContext.Context.SendActivityAsync("No catchers found for the specified repo key.", cancellationToken: cancellationToken);
                }
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Usage: ListCatchers /org/project/repo"), cancellationToken);
            } 
            

            return await stepContext.EndDialogAsync();
        }

        public async Task<DialogTurnResult> HandleHistoryCommand(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // TODO Create Command and Options 
            var parts = stepContext.Context.Activity.Text.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            int count = parts.Length > 1 ? int.Parse(parts[1]) : int.MaxValue;
            var history = await _pullPitchesTracking.History(count);
            if (history.IsNullOrEmpty())
            {
                await stepContext.Context.SendActivityAsync("No Pull Requests Assigned");
            }
            else
            {
                string historyMessage = string.Join("<br>", history.Select(h => $"<br>Repo: {h.Repository}, <br>PR: {h.Link}, <br>Assigned to: {h.Reviewers.First().Catcher.Name}"));
                await stepContext.Context.SendActivityAsync(historyMessage, cancellationToken: cancellationToken);
            }
        
            return await stepContext.EndDialogAsync();
        }

        public async Task<DialogTurnResult> HandleMeCommand(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var assignments = await _pullPitchesTracking.Me(stepContext.Context.Activity.From.Id);

            if (assignments.IsNullOrEmpty())
            {
                await stepContext.Context.SendActivityAsync("You did not have any pull requests, please do more efforts or you will be replaced by Smart AI Bot Like Me :)");
            }
            else
            {
                string assignmentsMessage = string.Join("<br>", assignments.Select(a => $"<br>Repo: {a.Repository} <br>PR: {a.Link}<br>Assigned to: {a.Reviewers.First().Catcher.Name}<br>----------"));
                await stepContext.Context.SendActivityAsync(assignmentsMessage, cancellationToken: cancellationToken);
            }
            return await stepContext.EndDialogAsync();
        }

        public async Task<DialogTurnResult> HandleToCommand(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var assignments = await _pullPitchesTracking.To(stepContext.Context.Activity.From.Id);
            if (assignments.IsNullOrEmpty())
            {
                await stepContext.Context.SendActivityAsync("You did not have any pull requests assigned to you, please do more efforts or you will be replaced by AI Bot Like Me :)");
            }
            else
            {
                string assignmentsMessage = string.Join("<br>", assignments.Select(a => $"Repo: {a.Repository}, PR: {a.Link}"));
                await stepContext.Context.SendActivityAsync(assignmentsMessage, cancellationToken: cancellationToken);
            }
           
            return await stepContext.EndDialogAsync();
        }
    }
}
