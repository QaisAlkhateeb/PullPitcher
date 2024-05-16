using System;
using System.Xml.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PullPitcher.Pulls
{
    public class Catcher : Entity<Guid>
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string ExternalId { get; protected set; }
        public string Repository { get; protected set; }
        protected Catcher() {}
        public Catcher(Guid id, string repo, string name, string email, string externalId)
        {
            Id = Check.NotDefaultOrNull<Guid>(id, nameof(id));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Email = Check.NotNullOrWhiteSpace(email, nameof(email));
            Repository = Check.NotNullOrWhiteSpace(repo, nameof(repo));
            ExternalId = Check.NotNullOrWhiteSpace(externalId, nameof(externalId));
        }
    }
}
