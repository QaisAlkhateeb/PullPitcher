using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PullPitcher.Pulls;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace PullPitcher.Controllers.Pulls
{
    [RemoteService(Name = "Pitcher")]
    [ControllerName("Pitcher")]
    [Route("api/[controller]")]
    public class PullRequestsController : PullPitcherController
    {
        private readonly IPullRequestAppService _pullRequestService;

        public PullRequestsController(IPullRequestAppService pullRequestAppService)
        {
            _pullRequestService = pullRequestAppService;
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
    }
}
