using PullPitcher.Contracts.Catchers;

namespace PullPitcher.Contracts.Pitchers
{
    public interface IPitcherAppService
    {
        Catcher PullPitch(string org, string project, string repo, int pullRequestId);
    }
}
