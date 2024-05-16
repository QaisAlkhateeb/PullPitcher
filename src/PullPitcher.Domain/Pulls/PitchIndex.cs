using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace PullPitcher.Pulls
{
    public class PitchIndex : Entity<string>
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
