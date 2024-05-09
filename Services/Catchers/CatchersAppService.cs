using PullPitcher.Contracts.Catchers;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace PullPitcher.Services.Catchers
{
    public class CatchersAppService : ICatchersAppService
    {
        private readonly Dictionary<string, List<Catcher>> repoUsers;
        private readonly Dictionary<string, int> repoIndex;

        public CatchersAppService(IConfiguration configuration)
        {
            repoUsers = new Dictionary<string, List<Catcher>>();
            repoIndex = new Dictionary<string, int>();
            var catchersSection = configuration.GetSection("Catchers");
            foreach (var child in catchersSection.GetChildren())
            {
                List<Catcher> users = child.Get<List<string>>()
                    .Select(name => new Catcher { Name = name }).ToList();
                repoUsers[child.Key] = users;
                repoIndex[child.Key] = 0;
            }
        }

        public List<Catcher> GetCatchers(string repoKey)
        {
            if (repoUsers.ContainsKey(repoKey))
            {
                return repoUsers[repoKey];
            }
            return new List<Catcher>();
        }

        public Catcher GetNextCatcher(string repoKey)
        {
            if (repoUsers.ContainsKey(repoKey) && repoUsers[repoKey].Count > 0)
            {
                int currentIndex = repoIndex[repoKey];
                List<Catcher> users = repoUsers[repoKey];
                Catcher assignedCatcher = users[currentIndex];
                // Update the index for the next assignment
                repoIndex[repoKey] = (currentIndex + 1) % users.Count;
                return assignedCatcher;
            }
            return null;
        }

        public void UpdateCatchers(string repoKey, List<Catcher> newCatchers)
        {
            repoUsers[repoKey] = newCatchers;
            repoIndex[repoKey] = 0; // Reset index whenever the list is updated
        }
    }
}
