using Discord.WebSocket;

namespace wesbot.SlashHandling
{
    public class AggregateSlashCommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IEnumerable<ISlashCommand> _handlers;

        public AggregateSlashCommandHandler(DiscordSocketClient client, IEnumerable<ISlashCommand> handlers)
        {
            _client = client;
            _handlers = handlers;
        }

        public async Task HandleSlashCommand(SocketSlashCommand command)
        {
            var handler = _handlers.FirstOrDefault(h => h.CommandName == command.Data.Name);
            if (handler != null)
            {
                await handler.Handle(command);
            }
        }
    }
}
