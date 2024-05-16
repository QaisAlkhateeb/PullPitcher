using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PullPitcher.Pulls
{
    public interface IPitcherAppService : ITransientDependency
    {
        Task<List<PullReviewerDto>> Pitch(string pullRequestLink, string ownerId);
    }
}
