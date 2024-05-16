using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PullPitcher.Pulls
{
    public class PullReviewer : Entity
    {
        public virtual PullRequest PullRequest { get; protected set; }
        public Guid PullRequestId { get; protected set; }

        public virtual Catcher Catcher { get; protected set; }
        public Guid CatcherId { get; protected set; }

        protected PullReviewer() { }

        public PullReviewer(Guid pullRequestId, Guid catcherId) 
        {
            PullRequestId = Check.NotDefaultOrNull<Guid>(pullRequestId, nameof(pullRequestId));
            CatcherId = Check.NotDefaultOrNull<Guid>(catcherId, nameof(catcherId));
        }

        public override object?[] GetKeys()
        {
            return new object?[] { PullRequestId, CatcherId };
        }
    }
}
