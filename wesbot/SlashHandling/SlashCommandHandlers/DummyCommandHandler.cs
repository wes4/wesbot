using Discord;
using Discord.WebSocket;

namespace wesbot.SlashHandling.SlashCommandHandlers
{
    public class DummyCommandHandler : ISlashCommand
    {
        public string CommandName => EnumExtensions.GetEnumDescription(SlashCommands.Dummy);

        public SlashCommandType CommandType => SlashCommandType.Global;

        public async Task Handle(SocketSlashCommand command)
        {
            await command.RespondAsync((string)command.Data.Options.First().Value);
        }

        public SlashCommandProperties Build(SlashCommandBuilder commandBuilder)
        {
            var description = EnumExtensions.GetEnumDescription(SlashCommands.Dummy);
            commandBuilder.WithName(description);
            commandBuilder.WithDescription("Dummy command.");
            commandBuilder.AddOption("text", ApplicationCommandOptionType.String, "Dummy command.", isRequired: true);

            return commandBuilder.Build();
        }
    }
}
