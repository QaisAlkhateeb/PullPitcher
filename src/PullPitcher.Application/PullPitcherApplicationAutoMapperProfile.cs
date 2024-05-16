using AutoMapper;
using PullPitcher.Pulls;

namespace PullPitcher;

public class PullPitcherApplicationAutoMapperProfile : Profile
{
    public PullPitcherApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Catcher, CatcherListItemDto>();
        CreateMap<SetCatcherDto, CatcherDetails>();

        CreateMap<PullRequest, PullRequestDto>();
        CreateMap<PullReviewer, PullReviewerDto>();
        CreateMap<Catcher, CatcherDto>();
    }
}
