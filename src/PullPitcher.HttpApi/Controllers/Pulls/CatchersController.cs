using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PullPitcher.Pulls;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace PullPitcher.Controllers.Pulls
{
    [RemoteService(Name = "Catchers")]
    [ControllerName("Catchers")]
    [Route("api/[controller]")]
    public class CatchersController : PullPitcherController
    {
        private readonly ICatcherAppService _catchersAppService;

        public CatchersController(ICatcherAppService catcherAppService)
        {
            _catchersAppService = catcherAppService;
        }

        [HttpPost("{repositoryKey}")]
        public async Task SetCatchers(string repositoryKey, List<SetCatcherDto> setCatcherDtos)
        {
            await _catchersAppService.SetCatchers(repositoryKey, setCatcherDtos);
        }

        [HttpGet("{repositoryKey}")]
        public async Task<List<CatcherListItemDto>> GetCatchers(string repositoryKey)
        {
            return await _catchersAppService.ListCatchers(repositoryKey);
        }
    }
}
