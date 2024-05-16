using PullPitcher.Samples;
using Xunit;

namespace PullPitcher.EntityFrameworkCore.Applications;

[Collection(PullPitcherTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<PullPitcherEntityFrameworkCoreTestModule>
{

}
