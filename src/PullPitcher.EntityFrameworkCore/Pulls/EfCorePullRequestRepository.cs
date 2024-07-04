using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.EntityFrameworkCore;
using PullPitcher.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PullPitcher.Pulls
{
    public class EfCorePullRequestRepository : EfCoreRepository<PullPitcherDbContext, PullRequest, Guid>, IRepository<PullRequest, Guid>
    {
        public EfCorePullRequestRepository(IDbContextProvider<PullPitcherDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<PullRequest>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(x => x.Reviewers).ThenInclude(e => e.Catcher).ThenInclude(c => c.Channel);
        }
    }
}
