using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PullPitcher.Pulls
{
    public class UserRef : Entity<Guid>
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string ExternalId { get; protected set; }
        protected UserRef() {}
        public UserRef(Guid id, string name, string email) 
            : base(id)
        {
            // TODO: validation
            Id = Check.NotDefaultOrNull<Guid>(id, nameof(id));
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Email = Check.NotNullOrWhiteSpace(email, nameof(email));
        }
    }
}
