using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PullPitcher.Pulls
{
    [RemoteService(false)]
    public class PullRequestAppService : ApplicationService, IPullRequestAppService
    {
        private readonly IRepository<PullRequest> _pullRequestsRepository;

        public PullRequestAppService(IRepository<PullRequest> pullRequestRepository)
        {
            _pullRequestsRepository = pullRequestRepository;
        }

        public async Task<List<PullRequestDto>> History(int pageSize)
        {

            var query = (await _pullRequestsRepository.WithDetailsAsync());
            var requests = query.Take(pageSize).ToList();
            return ObjectMapper.Map<List<PullRequest>, List<PullRequestDto>>(requests);
        }

        public async Task<List<PullRequestDto>> Me(string ownerId)
        {
            var query = (await _pullRequestsRepository.WithDetailsAsync());
            var requests = query.Where(p => p.OwnerId == ownerId).ToList();
            return ObjectMapper.Map<List<PullRequest>, List<PullRequestDto>>(requests);
        }

        public async Task<List<PullRequestDto>> To(string ownerId)
        {
            var query = (await _pullRequestsRepository.WithDetailsAsync());
            var requests = query.Where(p => p.Reviewers.Any(r => r.Catcher.ExternalId == ownerId)).ToList();
            return ObjectMapper.Map<List<PullRequest>, List<PullRequestDto>>(requests);
        }

        public async Task<List<PullRequestDto>> Waititng(int minutes = 240)
        {
            // TODO Move to shared query
            var query = (await _pullRequestsRepository.WithDetailsAsync());
            var threshold = DateTime.Now.AddMinutes(minutes * -1);
            var requests = query.Where(p => threshold >= p.CreationTime).ToList();
            return ObjectMapper.Map<List<PullRequest>, List<PullRequestDto>>(requests);
        }
    }
}
