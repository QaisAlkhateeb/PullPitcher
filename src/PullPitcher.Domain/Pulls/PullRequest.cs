using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PullPitcher.Pulls
{
    public class PullRequest : Entity<Guid>
    {
        public string OwnerId { get; protected set; }
        public string Repository { get; protected set; }
        public string Link { get; protected set; }
        public string Number { get; protected set; }
        public virtual List<PullReviewer> Reviewers { get; protected set; }

        protected PullRequest()
        {
            Reviewers = new List<PullReviewer>();
        }
        public PullRequest(Guid internalId, string number, string link, string ownerId, string repo) : 
            base(internalId)
        {
            Id = Check.NotDefaultOrNull<Guid>(internalId, nameof(internalId));
            OwnerId = Check.NotNullOrWhiteSpace(ownerId, nameof(ownerId));
            Repository = Check.NotNullOrWhiteSpace(repo, nameof(repo));
            Link = Check.NotNullOrWhiteSpace(link, nameof(link));
            Number = Check.NotNullOrWhiteSpace(number, nameof(number));
            Reviewers = new List<PullReviewer>();
        }

        public void AddReviewer(Guid catcherId)
        {
            Reviewers.Add(new PullReviewer(Id, catcherId));
        }
    }
}
