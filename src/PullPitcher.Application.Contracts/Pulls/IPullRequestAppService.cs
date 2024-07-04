using System.Collections.Generic;
using System.Threading.Tasks;

namespace PullPitcher.Pulls
{
    public interface IPullRequestAppService
    {
        /// <summary>
        /// Pull Requests Assigned to Me
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<List<PullRequestDto>> Me(string ownerId);

        /// <summary>
        /// Pull Requests I should Review
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<List<PullRequestDto>> To(string ownerId);
        Task<List<PullRequestDto>> History(int pageSize);
        Task<List<PullRequestDto>> Waititng(int minutes = 240);
    }
}
