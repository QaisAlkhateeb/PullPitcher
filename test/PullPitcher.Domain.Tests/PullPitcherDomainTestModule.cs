using Volo.Abp.Modularity;

namespace PullPitcher;

[DependsOn(
    typeof(PullPitcherDomainModule),
    typeof(PullPitcherTestBaseModule)
)]
public class PullPitcherDomainTestModule : AbpModule
{

}
