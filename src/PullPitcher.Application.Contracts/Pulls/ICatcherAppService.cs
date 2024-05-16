using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PullPitcher.Pulls
{
    public interface ICatcherAppService : ITransientDependency
    {
        Task SetCatchers(string repositoryKey, List<SetCatcherDto> catcherDtos);
        Task<List<CatcherListItemDto>> ListCatchers(string repositoryKey);
    }
}
