using Volo.Abp.Modularity;

namespace PullPitcher;

/* Inherit from this class for your domain layer tests. */
public abstract class PullPitcherDomainTestBase<TStartupModule> : PullPitcherTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
