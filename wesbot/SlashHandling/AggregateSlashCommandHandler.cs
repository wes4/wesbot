using Discord.WebSocket;

namespace wesbot.SlashHandling
{
    public class AggregateSlashCommandHandler
    {
        private readonly IEnumerable<ISlashCommand> _handlers;

        public AggregateSlashCommandHandler(IEnumerable<ISlashCommand> handlers)
        {
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
