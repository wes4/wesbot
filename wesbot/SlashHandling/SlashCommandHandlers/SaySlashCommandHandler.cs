using Discord;
using Discord.WebSocket;

namespace wesbot.SlashHandling.SlashCommandHandlers
{
    public class SaySlashCommandHandler : ISlashCommand
    {
        private readonly SlashCommandBuilder _builder;

        public string CommandName => EnumExtensions.GetEnumDescription(SlashCommands.Say);

        public SlashCommandType CommandType => SlashCommandType.Global;

        public SaySlashCommandHandler(SlashCommandBuilder builder) 
        {
            _builder = builder;
        }

        public async Task Handle(SocketSlashCommand command)
        {
            var responseText = (string)command.Data.Options.First().Value;
            await command.RespondAsync(responseText, ephemeral: false);
        }

        public SlashCommandProperties Build()
        {
            var description = EnumExtensions.GetEnumDescription(SlashCommands.Say);
            _builder.WithName(description);
            _builder.WithDescription("Replies with text.");
            _builder.AddOption("text", ApplicationCommandOptionType.String, "The text reply content.", isRequired: true);

            return _builder.Build();
        }
    }
}
