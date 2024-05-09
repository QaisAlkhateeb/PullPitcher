using System;
using Microsoft.Extensions.Logging;
using PullPitcher.Contracts.Catchers;
using PullPitcher.Contracts.Pitchers;

namespace PullPitcher.Services.Pitchers
{
    public partial class PitcherAppService : IPitcherAppService
    {
        private readonly ILogger<PitcherAppService> _logger;
        private readonly ICatchersAppService _catchersAppService;

        public PitcherAppService(ILogger<PitcherAppService> logger, ICatchersAppService catchersAppService)
        {
            // Initialize with dummy data for demonstration
            _logger = logger;
            _catchersAppService = catchersAppService;
        }

        public Catcher PullPitch(string org, string project, string repo, int pullRequestId)
        {
            string key = $"{org}/{project}/{repo}";
            Catcher assignedCatcher = _catchersAppService.GetNextCatcher(key);
            return assignedCatcher ?? throw new Exception("Repository not found or no users assigned.");
        }

    }
}
