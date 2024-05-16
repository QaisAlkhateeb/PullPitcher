using PullPitcher.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PullPitcher.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PullPitcherEntityFrameworkCoreModule),
    typeof(PullPitcherApplicationContractsModule)
    )]
public class PullPitcherDbMigratorModule : AbpModule
{
}
