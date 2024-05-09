// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.22.0

using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using PullPitcher.Contracts.Catchers;
using PullPitcher.Contracts.Pitchers;
using PullPitcher.Services.Catchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PullPitcher.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ILogger _logger;
        private readonly IPitcherAppService _pitcherAppService;
        private readonly ICatchersAppService _catchersAppService;
        // TODO: Move to settings
        private Regex _pullRequestRegex = new Regex(@"https:\/\/dev\.azure\.com\/([^\/]+)\/([^\/]+)\/_git\/([^\/]+)\/pullrequest\/(\d+)");
        private Dictionary<string, WaterfallStep> CommandsHandlerMap;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(ILogger<MainDialog> logger, IPitcherAppService pitcherAppService, ICatchersAppService catchersAppService)
            : base(nameof(MainDialog))
        {
            _logger = logger;
            _pitcherAppService = pitcherAppService;
            _catchersAppService = catchersAppService;

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
            
            await stepContext.Context.SendActivityAsync($"Command Not Found!");

            return await stepContext.EndDialogAsync();
        }

        private async Task<DialogTurnResult> HandleSetCatchersCommand(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var parts = stepContext.Context.Activity.Text.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3)
            {
                string repoKey = parts[1].Trim();
                string catcherEmails = parts.Length > 3 ? parts[3].Trim() : "";

                var members = await TeamsInfo.GetPagedMembersAsync(stepContext.Context);
                List<Catcher> newCatchers;

                if (string.IsNullOrEmpty(catcherEmails))
                {
                    newCatchers = members.Members.Select(member => new Catcher
                    {
                        Id = member.Id,
                        Name = member.Name,
                        Email = member.Email
                    }).ToList();
                }
                else
                {
                    var emailList = catcherEmails.Split(',').Select(email => email.Trim()).ToList();
                    newCatchers = members.Members.Where(member => emailList.Contains(member.Email)).Select(member => new Catcher
                    {
                        Id = member.Id,
                        Name = member.Name,
                        Email = member.Email
                    }).ToList();
                }

                _catchersAppService.UpdateCatchers(repoKey, newCatchers);
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
            var message = stepContext.Context.Activity.Text;
            Match match = _pullRequestRegex.Match(message);

            if (!match.Success && stepContext.Context.Activity.ChannelId == "msteams" && stepContext.Context.Activity.Attachments != null && stepContext.Context.Activity.Attachments.Count > 0)
            {
                message = stepContext.Context.Activity.Attachments[0].Content.ToString();
                match = _pullRequestRegex.Match(message);
            }

            if (match.Success)
            {
                var Organization = match.Groups[1].Value;
                var Project = match.Groups[2].Value;
                var Repo = match.Groups[3].Value;
                var PullRequestNumber = int.Parse(match.Groups[4].Value);

                var catcher = _pitcherAppService.PullPitch(Organization, Project, Repo, PullRequestNumber);

                var mention = new Mention
                {
                    // Mentioned = turnContext.Activity.From,
                    Mentioned = new ChannelAccount(catcher.Id, catcher.Name),
                    Text = $"<at>{XmlConvert.EncodeName(catcher.Name)}</at>",
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
                List<Catcher> catchers = _catchersAppService.GetCatchers(repoKey);

                if (catchers != null && catchers.Count > 0)
                {
                    string messageText = "Catchers for repo '" + repoKey + "':\n" + string.Join("\n", catchers.Select(c => $"{c.Name} ({c.Email})"));
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
    }
}
