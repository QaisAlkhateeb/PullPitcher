using PullPitcher.Contracts.Catchers;

namespace PullPitcher.Contracts.Pitchers
{
    public interface IPullPitchesTracking
    {
        Catcher GetAssignedCatcher(string repoKey, int pullRequestId);
        void TrackAssignment(string repoKey, int pullRequestId, Catcher catcher);
        void ClearHistory();
    }

}
