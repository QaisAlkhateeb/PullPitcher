using System;
using Volo.Abp.Application.Dtos;

namespace PullPitcher.Pulls
{
    public class CatcherListItemDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }
        public string Repository { get; set; }
    }
}