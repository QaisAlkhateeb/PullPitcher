using PullPitcher.Contracts.Catchers;
using PullPitcher.Services.Pitchers;
using System.Collections.Generic;

namespace PullPitcher.Contracts.Pitchers
{
    public interface IPullPitchesTracking
    {
        Catcher GetAssignedCatcher(string repoKey, int pullRequestId);
        void TrackAssignment(string repoKey, int pullRequestId, Catcher catcher);
        void ClearHistory();

        List<PullTracking> GetHistory(int count);
        List<PullTracking> GetAssignmentsForCatcher(string catcherId);
    }

}
