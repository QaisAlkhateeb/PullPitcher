using Volo.Abp.Modularity;

namespace PullPitcher;

[DependsOn(
    typeof(PullPitcherApplicationModule),
    typeof(PullPitcherDomainTestModule)
)]
public class PullPitcherApplicationTestModule : AbpModule
{

}
