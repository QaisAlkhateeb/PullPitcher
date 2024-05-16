using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace PullPitcher.Pages;

public class Index_Tests : PullPitcherWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
