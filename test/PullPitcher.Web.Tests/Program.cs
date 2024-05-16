using Microsoft.AspNetCore.Builder;
using PullPitcher;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<PullPitcherWebTestModule>();

public partial class Program
{
}
