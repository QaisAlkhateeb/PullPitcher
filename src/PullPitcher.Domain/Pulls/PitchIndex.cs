using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace PullPitcher.Pulls
{
    public class PitchIndex : CreationAuditedEntity<string>
    {
        public int Index { get; protected set; }
        protected PitchIndex()
        {
            
        }
        public PitchIndex(string Repository, int index)
        {
            Id = Check.NotNullOrWhiteSpace(Repository, nameof(Repository));
            Index = index;
        }

        public void SetIndex(int index)
        {
            Index = index;
        }
    }
}
