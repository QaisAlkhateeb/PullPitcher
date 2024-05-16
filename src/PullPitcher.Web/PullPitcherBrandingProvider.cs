using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace PullPitcher.Web;

[Dependency(ReplaceServices = true)]
public class PullPitcherBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "PullPitcher";
}
