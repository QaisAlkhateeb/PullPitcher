﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace PullPitcher.Pulls
{
    public class CatchersManager : DomainService
    {
        private readonly IRepository<Catcher, Guid> _catcherRepository;
        private readonly IRepository<PitchIndex, string> _pitchIndexRepository;
        private readonly IRepository<Channel, string> _channelsRepository;

        public CatchersManager(IRepository<Catcher, Guid> catcherRepository,
            IRepository<PitchIndex, string> pitchIndexRepository,
            IRepository<Channel, string> channelsRepository)
        {
            _catcherRepository = catcherRepository;
            _pitchIndexRepository = pitchIndexRepository;
            _channelsRepository = channelsRepository;
        }
        public async Task SetCatchers(string botId, string conversationId, string repoKey, List<CatcherDetails> newCatchers)
        {
            // Create Channel If not exists 
            var channel = await _channelsRepository.FindAsync(c => c.BotId == botId && c.Id == conversationId);
            if (channel == null)
            {
                channel = new Channel(conversationId, botId);
                await _channelsRepository.InsertAsync(channel, true);
            }

            // Remove existing catchers
            await _catcherRepository.DeleteAsync(c => c.Repository == repoKey, true);

            // Add new catchers
            var mapCatchers = newCatchers
                .Select(c => new Catcher(GuidGenerator.Create(), channel.Id, repoKey, c.Name, c.Email, c.ExternalId))
                .OrderBy(x => Guid.NewGuid()); //shuffle

            await _catcherRepository.InsertManyAsync(mapCatchers);

            var pitchIndex = await _pitchIndexRepository.FindAsync(repoKey);
            if (pitchIndex == null)
            {
                pitchIndex = new PitchIndex(repoKey, 0);
                await _pitchIndexRepository.InsertAsync(pitchIndex);
            }
            else
            {
                pitchIndex.SetIndex(0);
            }
        }
        public async Task<List<Catcher>> GetCatchers(string repository)
        {
            return await _catcherRepository.GetListAsync(c => c.Repository == repository);
        }
    }

    public class CatcherDetails
    {
        // TODO Validation
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string ExternalId { get; protected set; }
    }
}
