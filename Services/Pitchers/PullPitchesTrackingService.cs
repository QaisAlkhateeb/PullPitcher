using PullPitcher.Contracts.Catchers;
using PullPitcher.Contracts.Pitchers;
using System.Collections.Generic;
using System.Linq;

namespace PullPitcher.Services.Pitchers
{
    public class PullPitchesTrackingService : IPullPitchesTracking
    {
        private readonly List<PullTracking> _assignmentHistory;
        private readonly int _historySize;

        public PullPitchesTrackingService(PitcherSettings pitcherSettings)
        {
            _historySize = pitcherSettings.AssignmentHistorySize;
            _assignmentHistory = new List<PullTracking>();
        }

        public Catcher GetAssignedCatcher(string repoKey, int pullRequestId)
        {
            return _assignmentHistory.FirstOrDefault(e => e.RepoKey == repoKey && e.PullRequestId == pullRequestId)?.Catcher;
        }

        public void TrackAssignment(string repoKey, int pullRequestId, Catcher catcher)
        {
            if (_assignmentHistory.Count >= _historySize)
            {
                _assignmentHistory.RemoveAt(0);
            }
            _assignmentHistory.Add(new PullTracking
            {
                Catcher = catcher,
                RepoKey = repoKey,
                PullRequestId = pullRequestId
            });
        }

        public void ClearHistory()
        {
            _assignmentHistory.Clear();
        }

        public List<PullTracking> GetHistory(int count)
        {
            return _assignmentHistory.TakeLast(count).ToList();
        }

        public List<PullTracking> GetAssignmentsForCatcher(string catcherId)
        {
            return _assignmentHistory.Where(x => x.Catcher.Id == catcherId).ToList();
        }
    }

    public class PullTracking
    {
        public int PullRequestId { get; set; }
        public Catcher Catcher { get; set; }
        public string RepoKey { get; set; }
    }

}
