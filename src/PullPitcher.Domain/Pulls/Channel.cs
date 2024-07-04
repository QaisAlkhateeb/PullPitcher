using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace PullPitcher.Pulls
{
    public class Channel : CreationAuditedEntity<string>
    {
        public string ChannelId { get; set; }
        public string ServiceUrl { get; set; }
        public string BotId { get; set; }

        protected Channel() { }
        public Channel(string conversationId, string botId, string serviceUrl = "https://smba.trafficmanager.net/emea/", string channelId = "msteams")
        {
            Id  = Check.NotNullOrWhiteSpace(conversationId, nameof(conversationId));
            BotId = Check.NotNullOrWhiteSpace(botId, nameof(botId));
            ServiceUrl = Check.NotNullOrWhiteSpace(serviceUrl, nameof(serviceUrl));
            ChannelId = Check.NotNullOrWhiteSpace(channelId, nameof(channelId));
        }
    }
}
