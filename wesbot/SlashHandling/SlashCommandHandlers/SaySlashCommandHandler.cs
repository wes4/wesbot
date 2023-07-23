using Discord;
using Discord.WebSocket;

namespace wesbot.SlashHandling.SlashCommandHandlers
{
    public class SaySlashCommandHandler : ISlashCommand
    {
        public string CommandName => EnumExtensions.GetEnumDescription(SlashCommands.Say);

        public SlashCommandType CommandType => SlashCommandType.Global;

        public async Task Handle(SocketSlashCommand command)
        {
            var responseText = (string)command.Data.Options.First().Value;
            await command.RespondAsync(responseText, ephemeral: false);
        }

        public SlashCommandProperties Build(SlashCommandBuilder commandBuilder)
        {
            var description = EnumExtensions.GetEnumDescription(SlashCommands.Say);
            commandBuilder.WithName(description);
            commandBuilder.WithDescription("Replies with text.");
            commandBuilder.AddOption("text", ApplicationCommandOptionType.String, "The text reply content.", isRequired: true);

            return commandBuilder.Build();
        }
    }
}
