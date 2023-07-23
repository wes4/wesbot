using Discord;
using Discord.WebSocket;

namespace wesbot.SlashHandling.SlashCommandHandlers
{
    public class DummyCommandHandler : ISlashCommand
    {
        private SlashCommandBuilder _builder;

        public string CommandName => EnumExtensions.GetEnumDescription(SlashCommands.Dummy);

        public SlashCommandType CommandType => SlashCommandType.Global;

        public DummyCommandHandler(SlashCommandBuilder builder)
        {
            _builder = builder;
        }

        public async Task Handle(SocketSlashCommand command)
        {
            await command.RespondAsync((string)command.Data.Options.First().Value);
        }

        public SlashCommandProperties Build()
        {
            var description = EnumExtensions.GetEnumDescription(SlashCommands.Dummy);
            _builder.WithName(description);
            _builder.WithDescription("Dummy command.");
            _builder.AddOption("text", ApplicationCommandOptionType.String, "Dummy command.", isRequired: true);

            return _builder.Build();
        }
    }
}
