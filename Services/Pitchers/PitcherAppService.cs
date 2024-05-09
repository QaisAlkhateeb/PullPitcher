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
        private readonly IPullPitchesTracking _pitchTracking;

        public PitcherAppService(ILogger<PitcherAppService> logger, ICatchersAppService catchersAppService, IPullPitchesTracking pitchTracking)
        {
            _logger = logger;
            _catchersAppService = catchersAppService;
            _pitchTracking = pitchTracking;
        }

        public Catcher PullPitch(string org, string project, string repo, int pullRequestId)
        {
            string key = $"{org}/{project}/{repo}";
            Catcher existingCatcher = _pitchTracking.GetAssignedCatcher(key, pullRequestId);

            if (existingCatcher != null)
            {
                return existingCatcher;
            }

            Catcher assignedCatcher = _catchersAppService.GetNextCatcher(key);
            if (assignedCatcher == null)
            {
                throw new Exception("Repository not found or no users assigned.");
            }

            _pitchTracking.TrackAssignment(key, pullRequestId, assignedCatcher);
            return assignedCatcher;
        }

    }
}
