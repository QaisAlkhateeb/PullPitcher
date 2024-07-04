using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PullPitcher.Pulls;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp;
using static System.Collections.Specialized.BitVector32;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace PullPitcher.Controllers.Pulls
{
    [RemoteService(Name = "Pitcher")]
    [ControllerName("Pitcher")]
    [Route("api/[controller]")]
    public class PullRequestsController : PullPitcherController
    {
        private readonly IPullRequestAppService _pullRequestService;
        private readonly IProactiveMessageSender _proactiveMessageSender;
        private readonly ICatcherAppService _catchersAppService;

        public PullRequestsController(IPullRequestAppService pullRequestAppService, IProactiveMessageSender proactiveMessageSender, ICatcherAppService catcherAppService)
        {
            _pullRequestService = pullRequestAppService;
            _proactiveMessageSender = proactiveMessageSender;
            _catchersAppService = catcherAppService;
        }

        [HttpGet("Me/{ownerId}")]
        public async Task<List<PullRequestDto>> Me(string ownerId)
        {
            return await _pullRequestService.Me(ownerId);
        }

        [HttpGet("To/{ownerId}")]
        public async Task<List<PullRequestDto>> To(string ownerId)
        {
            return await _pullRequestService.To(ownerId);
        }

        [HttpGet("History")]
        public async Task<List<PullRequestDto>> History(int pageSize = 20)
        {
            return await _pullRequestService.History(pageSize);
        }

        [HttpPost("Notify")]
        public async Task Notify(int minutes = 240)
        {
            var requests = await _pullRequestService.Waititng(minutes);
            if (!requests.IsNullOrEmpty())
            {
                string historyMessage = string.Join("<br>", requests.Select(h => $"<br>Repo: {h.Repository}, <br>PR: {h.Link}, <br>Assigned to: {h.Reviewers.First().Catcher.Name}"));

                var users = requests.Select(r => r.Reviewers.FirstOrDefault().Catcher);
                var groupedByChannel = users.GroupBy(u => u.ChannelId);
                // TODOD Don't repeat messages
                foreach ( var group in groupedByChannel)
                {
                    var activity = MessageFactory.Text(historyMessage);
                    activity.Entities = new List<Entity> { };
                    foreach (var user in group.Select(g => g))
                    {
                        var mention = new Mention
                        {
                            // Mentioned = turnContext.Activity.From,
                            Mentioned = new ChannelAccount(user.ExternalId, user.Name),
                            Text = $"<at>{XmlConvert.EncodeName(user.Name)}</at>",
                        };
                        activity.Entities.Add(mention);
                    }
                    await _proactiveMessageSender.SendMessage("NA", group.Key, activity);
                }
                
            }
        }
    }
}
