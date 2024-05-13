using System.Collections.Generic;

namespace PullPitcher.Contracts.Catchers
{
    public interface ICatchersAppService
    {
        List<Catcher> GetCatchers(string repoKey);
        Catcher GetNextCatcher(string repoKey, string ownerId);
        void UpdateCatchers(string repoKey, List<Catcher> newCatchers);

    }

}
