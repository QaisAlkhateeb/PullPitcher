using PullPitcher.Samples;
using Xunit;

namespace PullPitcher.EntityFrameworkCore.Domains;

[Collection(PullPitcherTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<PullPitcherEntityFrameworkCoreTestModule>
{

}
