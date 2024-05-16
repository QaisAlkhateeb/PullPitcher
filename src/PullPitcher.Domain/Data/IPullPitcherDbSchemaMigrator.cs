using System.Threading.Tasks;

namespace PullPitcher.Data;

public interface IPullPitcherDbSchemaMigrator
{
    Task MigrateAsync();
}
