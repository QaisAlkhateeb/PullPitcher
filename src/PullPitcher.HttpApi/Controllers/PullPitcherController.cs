using PullPitcher.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PullPitcher.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class PullPitcherController : AbpControllerBase
{
    protected PullPitcherController()
    {
        LocalizationResource = typeof(PullPitcherResource);
    }
}
