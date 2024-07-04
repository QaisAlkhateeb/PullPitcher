using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace PullPitcher.Pulls
{
    [RemoteService(false)]
    public class CatcherAppService : ApplicationService, ICatcherAppService
    {
        private readonly CatchersManager _catchersManager;

        public CatcherAppService(CatchersManager catchersManager)
        {
            _catchersManager = catchersManager;
        }
        public async Task<List<CatcherListItemDto>> ListCatchers(string repositoryKey)
        {
            var catchersList = await _catchersManager.GetCatchers(repositoryKey);
            var catchersMapped = ObjectMapper.Map<List<Catcher>, List<CatcherListItemDto>>(catchersList);
            return catchersMapped;
        }

        public async Task SetCatchers(string botId, string conversationId, string repositoryKey, List<SetCatcherDto> catcherDtos)
        {
            var mapped = ObjectMapper.Map<List<SetCatcherDto>, List<CatcherDetails>>(catcherDtos);
            await _catchersManager.SetCatchers(botId, conversationId, repositoryKey, mapped);
        }
    }
}
