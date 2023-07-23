using Discord;
using Discord.WebSocket;

namespace wesbot.SlashHandling
{
    public interface ISlashCommand
    {
        string CommandName { get; }

        SlashCommandType CommandType { get; }

        Task Handle(SocketSlashCommand command);

        SlashCommandProperties Build(SlashCommandBuilder commandBuilder);
    }
}
