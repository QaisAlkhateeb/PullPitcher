using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PullPitcher.Data;
using Volo.Abp.DependencyInjection;

namespace PullPitcher.EntityFrameworkCore;

public class EntityFrameworkCorePullPitcherDbSchemaMigrator
    : IPullPitcherDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCorePullPitcherDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the PullPitcherDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<PullPitcherDbContext>()
            .Database
            .MigrateAsync();
    }
}
