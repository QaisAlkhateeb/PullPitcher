using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace PullPitcher.Pulls
{
    [RemoteService(false)]
    public class PitcherAppService : ApplicationService, IPitcherAppService
    {
        private Pitcher _pitcher;
        // TODO: Move to settings
        private Regex _pullRequestRegex = new Regex(@"https:\/\/dev\.azure\.com\/([^\/]+)\/([^\/]+)\/_git\/([^\/]+)\/pullrequest\/(\d+)");
        public PitcherAppService(Pitcher pitcher)
        {
            _pitcher = pitcher;
        }
        public async Task<List<PullReviewerDto>> Pitch(string pullRequestLink, string ownerId)
        {
            // TODO Create Parser
            Match match = _pullRequestRegex.Match(pullRequestLink);
            if (match.Success)
            {
                var Organization = match.Groups[1].Value;
                var Project = match.Groups[2].Value;
                var Repo = match.Groups[3].Value;
                var PullRequestNumber = int.Parse(match.Groups[4].Value);
                string key = $"{Organization}*{Project}*{Repo}";

                var reviewers = await _pitcher.Pitch(pullRequestLink, key, ownerId, PullRequestNumber.ToString());

                return ObjectMapper.Map<List<PullReviewer>, List<PullReviewerDto>>(reviewers);
            }
            else
            {
                throw new BusinessException(message: "Invalid Pull Request Link");
            }
        }
    }
}
