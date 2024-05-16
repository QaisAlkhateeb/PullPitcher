using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace PullPitcher.Pulls
{
    public class Pitcher : DomainService
    {
        private readonly IRepository<Catcher, Guid> _catcherRepository;
        private readonly IRepository<PitchIndex, string> _pitchIndexRepository;
        private readonly IRepository<PullRequest, Guid> _pullRequestRepository; // Added repository for PullRequest

        public Pitcher(IRepository<Catcher, Guid> catcherRepository,
                       IRepository<PitchIndex, string> pitchIndexRepository,
                       IRepository<PullRequest, Guid> pullRequestRepository) // Inject repository for PullRequest
        {
            _catcherRepository = catcherRepository;
            _pitchIndexRepository = pitchIndexRepository;
            _pullRequestRepository = pullRequestRepository;
        }

        public async Task<List<PullReviewer>> Pitch(string link, string repoKey, string ownerId, string pullRequestId)
        {
            // Retrieve the pull request
            var pullRequest = (await _pullRequestRepository.WithDetailsAsync())
                .FirstOrDefault(p => p.Number ==  pullRequestId);
            if (pullRequest != null)
            {
                return pullRequest.Reviewers;
            }
            
            // Initialize Pull Request
            pullRequest = new PullRequest(GuidGenerator.Create(), pullRequestId, link, ownerId, repoKey);

            // Ensure the owner of the pull request is not getting assigned
            var catchers = await _catcherRepository.GetListAsync(c => c.Repository == repoKey);
            if (catchers.Count <= 1)
            {
                throw new BusinessException($"Not Catchers Found Match Repository {repoKey}");
            }

            var pitchIndex = await _pitchIndexRepository.FindAsync(repoKey);
            if (pitchIndex == null)
            {
                pitchIndex = new PitchIndex(repoKey, 0);
                await _pitchIndexRepository.InsertAsync(pitchIndex);
            }

            int startIndex = pitchIndex.Index;
            int totalCatchers = catchers.Count;
            int attempts = 0;

            while (attempts < totalCatchers)
            {
                Catcher catcher = catchers[startIndex % totalCatchers];
                startIndex = (startIndex + 1) % totalCatchers; // move index forward for next time

                if (catcher.ExternalId != ownerId)
                {
                    pitchIndex.SetIndex(startIndex); // update the index to the next catcher for future calls

                    // Add or update the reviewer record
                    pullRequest.AddReviewer(catcher.Id);

                    await _pullRequestRepository.InsertAsync(pullRequest);

                    return pullRequest.Reviewers;
                }
                attempts++;
            }

            throw new BusinessException("No eligible catcher found that is not the owner.");
        }
    }
}
