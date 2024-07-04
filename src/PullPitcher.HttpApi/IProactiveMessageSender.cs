using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PullPitcher
{
    // This should be moved to Bot Abstraction
    public interface IProactiveMessageSender : ITransientDependency
    {
        Task SendMessage(string botId, string conversation, Activity activity); 
    }

    public class ProactiveMessageSender : IProactiveMessageSender
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string _appId;

        public ProactiveMessageSender(IBotFrameworkHttpAdapter adapter, IConfiguration configuration)
        {
            _adapter = adapter;
            _appId = configuration["MicrosoftAppId"];
        }
        public async Task SendMessage(string botId, string conversation, Activity activity)
        {
            var conversationReference = new ConversationReference
            {
                ServiceUrl = "https://smba.trafficmanager.net/emea/",
                ChannelId = "msteams",
                Conversation = new ConversationAccount
                {
                    Id = conversation
                },
                Bot = new ChannelAccount
                {
                    Id = _appId
                }
            };

            await((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationReference, async (context, token) =>
            {
                await context.SendActivityAsync(activity, token);
            }, default);
        }
    }
}
