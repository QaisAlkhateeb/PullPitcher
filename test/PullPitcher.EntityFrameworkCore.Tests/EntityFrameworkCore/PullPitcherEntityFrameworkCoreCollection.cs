using Xunit;

namespace PullPitcher.EntityFrameworkCore;

[CollectionDefinition(PullPitcherTestConsts.CollectionDefinitionName)]
public class PullPitcherEntityFrameworkCoreCollection : ICollectionFixture<PullPitcherEntityFrameworkCoreFixture>
{

}
