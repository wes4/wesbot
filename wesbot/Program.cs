using Discord;
using Discord.Commands;
using Discord.WebSocket;
using wesbot.PrefixHandling;
using wesbot.SlashHandling;
using Microsoft.Extensions.DependencyInjection;
using Discord.Net;
using Newtonsoft.Json;
using System;

namespace wesbot
{
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly IServiceProvider _services;

        private Program()
        {
            _services = ConfigureServices();
        }

        private IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                .AddSingleton<DiscordSocketClient>(client =>
                {
                    return new DiscordSocketClient(new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Info,

                        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,

                        // If you or another service needs to do anything with messages
                        // (eg. checking Reactions, checking the content of edited/deleted messages),
                        // you must set the MessageCacheSize. You may adjust the number as needed.
                        //MessageCacheSize = 50,

                        // If your platform doesn't have native WebSockets,
                        // add Discord.Net.Providers.WS4Net from NuGet,
                        // add the `using` at the top, and uncomment this line:
                        //WebSocketProvider = WS4NetProvider.Instance
                    });
                })
                .AddSingleton<CommandService>(commandHandler =>
                {
                    return new CommandService(new CommandServiceConfig
                    {
                        LogLevel = LogSeverity.Info,
                        CaseSensitiveCommands = false,
                    });
                })
                .AddSingleton<CommandHandler>()
                .AddSingleton<AggregateSlashCommandHandler>()
                .AddTransient<SlashCommandBuilder>();

            // Dynamically load all implementations of ISlashCommandHandler
            // NOTE: This implementation requires all ISlashCommandHandler to be in the same assembly as Program.cs
            var handlers = typeof(Program).Assembly.GetTypes().Where(x => typeof(ISlashCommand).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract);
            foreach (var handler in handlers)
            {
                map.Add(new ServiceDescriptor(typeof(ISlashCommand), handler, ServiceLifetime.Singleton));
            }

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            return map.BuildServiceProvider();
        }

        // Example of a logging handler. This can be re-used by addons
        // that ask for a Func<LogMessage, Task>.
        private static Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        private async Task MainAsync()
        {
            var commands = _services.GetRequiredService<CommandService>();
            var client = _services.GetRequiredService<DiscordSocketClient>();

            client.Ready += OnClientReady;
            client.Log += Log;
            commands.Log += Log;

            // Login and start bot
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
            await client.StartAsync();

            // Hook up handlers
            await _services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
            _services.GetRequiredService<AggregateSlashCommandHandler>().SetupSlashCommandsHandle();

            // Wait infinitely so your bot actually stays connected.
            await Task.Delay(Timeout.Infinite);
        }

        private async Task OnClientReady()
        {
            var client = _services.GetRequiredService<DiscordSocketClient>();
            var slashCommands = _services.GetRequiredService<IEnumerable<ISlashCommand>>();
            foreach (var command in slashCommands)
            {
                try
                {
                    if (command.CommandType == SlashCommandType.Global)
                    {
                        // await client.CreateGlobalApplicationCommandAsync(command.Build(_services.GetRequiredService<SlashCommandBuilder>()));
                    }
                    else
                    {
                        //// TODO: GuildId?
                        //var guild = client.GetGuild(guildId);
                        //await guild.CreateApplicationCommandAsync(command.Build(_services.GetRequiredService<SlashCommandBuilder>()));
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
