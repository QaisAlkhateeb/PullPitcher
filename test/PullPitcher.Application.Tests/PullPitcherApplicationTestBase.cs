using Volo.Abp.Modularity;

namespace PullPitcher;

public abstract class PullPitcherApplicationTestBase<TStartupModule> : PullPitcherTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
