using PullPitcher.Contracts.Catchers;
using PullPitcher.Contracts.Pitchers;
using System.Collections.Generic;
using System.Linq;

namespace PullPitcher.Services.Pitchers
{
    public class PullPitchesTrackingService : IPullPitchesTracking
    {
        private readonly Dictionary<(string, int), Catcher> _assignmentHistory;
        private readonly int _historySize;

        public PullPitchesTrackingService(PitcherSettings pitcherSettings)
        {
            _historySize = pitcherSettings.AssignmentHistorySize;
            _assignmentHistory = new Dictionary<(string, int), Catcher>();
        }

        public Catcher GetAssignedCatcher(string repoKey, int pullRequestId)
        {
            (string, int) key = (repoKey, pullRequestId);
            if (_assignmentHistory.TryGetValue(key, out Catcher catcher))
            {
                return catcher;
            }
            return null;
        }

        public void TrackAssignment(string repoKey, int pullRequestId, Catcher catcher)
        {
            (string, int) key = (repoKey, pullRequestId);
            if (_assignmentHistory.Count >= _historySize)
            {
                var oldestKey = _assignmentHistory.Keys.First();
                _assignmentHistory.Remove(oldestKey);
            }
            _assignmentHistory[key] = catcher;
        }

        public void ClearHistory()
        {
            _assignmentHistory.Clear();
        }
    }

}
