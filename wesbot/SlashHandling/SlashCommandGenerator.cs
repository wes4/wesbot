using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace wesbot.SlashHandling
{
    public class SlashCommandGenerator
    {
        private DiscordSocketClient _client; 
        private IEnumerable<ISlashCommand> _commands;

        public SlashCommandGenerator(DiscordSocketClient client, IEnumerable<ISlashCommand> commands) 
        { 
            _client = client;
            _commands = commands;
        }

        public async Task GenerateCommands()
        {
            foreach (var command in _commands)
            {
                try
                {
                    if (command.CommandType == SlashCommandType.Global)
                    {
                        var properties = command.Build();
                        // await _client.CreateGlobalApplicationCommandAsync(properties);
                    }
                    else
                    {
                        // TODO: Create Guild Application Command
                    }
                }
                catch (HttpException exception)
                {
                    // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                    var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                    // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                    Console.WriteLine(json);
                }
            }
        }
    }
}
