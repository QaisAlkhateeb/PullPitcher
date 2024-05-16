using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PullPitcher.Pulls;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace PullPitcher.Controllers.Pulls
{
    [RemoteService(Name = "Pitcher")]
    [ControllerName("Pitcher")]
    [Route("api/[controller]")]
    public class PitcherController : PullPitcherController
    {
        private readonly IPitcherAppService _pitcherAppservice;

        public PitcherController(IPitcherAppService pitcherAppService)
        {
            _pitcherAppservice = pitcherAppService;
        }
        [HttpPost]
        public Task<List<PullReviewerDto>> Pitch(PitchInput pitchInput) 
        {
            return _pitcherAppservice.Pitch(pitchInput.Link, pitchInput.OwnerId);
        }
    }
}
