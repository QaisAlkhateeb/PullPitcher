using PullPitcher.Contracts.Catchers;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using PullPitcher.Exceptions;

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

        public Catcher GetNextCatcher(string repoKey, string ownerId)
        {
            if (!repoUsers.ContainsKey(repoKey))
            {
                throw new BusinessException("Repository not found or no users assigned.");
            }

            List<Catcher> catchers = repoUsers[repoKey];
            if (catchers.Count <= 1)
            {
                throw new BusinessException("Not enough users to assign a different catcher.");
            }

            int totalCatchers = catchers.Count;
            int startIndex = repoIndex[repoKey];
            int attempts = 0;

            while (attempts < totalCatchers)
            {
                Catcher catcher = catchers[startIndex];
                startIndex = (startIndex + 1) % totalCatchers; // move index forward for next time
                if (catcher.Id != ownerId)
                {
                    repoIndex[repoKey] = startIndex; // update the index to the next catcher for future calls
                    return catcher;
                }
                attempts++;
            }

            throw new BusinessException("No eligible catcher found that is not the owner.");
        }

        public void UpdateCatchers(string repoKey, List<Catcher> newCatchers)
        {
            repoUsers[repoKey] = newCatchers;
            repoIndex[repoKey] = 0; // Reset index whenever the list is updated
        }
    }
}
