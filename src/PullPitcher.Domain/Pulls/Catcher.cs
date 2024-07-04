using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PullPitcher.Pulls
{
    public class Catcher : CreationAuditedEntity<Guid>
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string ExternalId { get; protected set; }
        public string Repository { get; protected set; }

        public virtual Channel Channel { get; protected set; }
        public string ChannelId { get; protected set; }
        protected Catcher() {}
        public Catcher(Guid id, string channelId, string repo, string name, string email, string externalId)
        {
            Id = Check.NotDefaultOrNull<Guid>(id, nameof(id));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Email = Check.NotNullOrWhiteSpace(email, nameof(email));
            Repository = Check.NotNullOrWhiteSpace(repo, nameof(repo));
            ExternalId = Check.NotNullOrWhiteSpace(externalId, nameof(externalId));
            ChannelId = Check.NotNullOrWhiteSpace(channelId, nameof(channelId));
        }
    }
}
