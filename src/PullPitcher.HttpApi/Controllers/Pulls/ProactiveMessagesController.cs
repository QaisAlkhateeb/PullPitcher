using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PullPitcher.Controllers.Pulls
{
    [ApiController]
    [Route("api/messages/proactive")]
    public class ProactiveMessagesController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly string _appId;

        public ProactiveMessagesController(IBotFrameworkHttpAdapter adapter, IConfiguration configuration)
        {
            _adapter = adapter;
            _appId = configuration["MicrosoftAppId"];
        }

        [HttpPost]
        public async Task<IActionResult> SendProactiveMessage([FromBody] ProactiveMessageData data)
        {
            var conversationReference = new ConversationReference
            {
                ServiceUrl = data.ServiceUrl,
                ChannelId = data.ChannelId,
                Conversation = new ConversationAccount
                {
                    Id = data.ConversationId
                },
                Bot = new ChannelAccount
                {
                    Id = _appId
                },
            };

            await ((BotAdapter)_adapter).ContinueConversationAsync(_appId, conversationReference, async (context, token) =>
            {
                var reply = MessageFactory.Text(data.Message);
                await context.SendActivityAsync(reply, token);
            }, default);

            return Ok();
        }
    }

    public class ProactiveMessageData
    {
        public string ServiceUrl { get; set; }
        public string ChannelId { get; set; }
        public string ConversationId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
