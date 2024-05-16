using PullPitcher.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PullPitcher.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class PullPitcherPageModel : AbpPageModel
{
    protected PullPitcherPageModel()
    {
        LocalizationResourceType = typeof(PullPitcherResource);
    }
}
