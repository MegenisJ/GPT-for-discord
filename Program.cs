using GPT_for_discord.Services.Interfaces;
using GPT_for_discord.Services;
using Newtonsoft.Json.Linq;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions.Common;
using System;
using GPT_for_discord.Services;
public class Program
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DiscordSocketClient discordClient;
    IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

    public Program()
    {
        _serviceProvider = GPT_for_discord.Services.ServiceProvider.CreateServices();
        discordClient = _serviceProvider.GetRequiredService<DiscordSocketClient>();
    }

    static void Main(string[] args)
        => new Program().RunAsync(args).GetAwaiter().GetResult();

    async Task RunAsync(string[] args)
    {
        discordClient.Log += async (msg) =>
        {
            await Task.CompletedTask;
            Console.WriteLine(msg);
        };

        await discordClient.LoginAsync(TokenType.Bot, config.GetValue<string>("Discord:token"));
        await discordClient.StartAsync();
        discordClient.Ready += CreateSlashCommands;
        discordClient.SlashCommandExecuted += SlashCommandHandler;
        await Task.Delay(Timeout.Infinite);
    }

    private async Task CreateSlashCommands()
    {
        var command = new SlashCommandBuilder()
        {
            Name = "mejai",
            Description = "ask me anything!",
            Options = new List<SlashCommandOptionBuilder> { new SlashCommandOptionBuilder() { 
                Name= "message", 
                Type = ApplicationCommandOptionType.String, 
                IsRequired = true, 
                Description = "message send to mej ai"
            } }
            
        };
        await discordClient.CreateGlobalApplicationCommandAsync(command.Build());
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        await command.RespondAsync(command.User.Username + " asked :" + command.Data.Options.First().Value.ToString());
        IOpenAIProxy chatOpenAI = new OpenAIProxy(
            apiKey: config.GetValue<string>("GPT:apiKey"),
            organizationId: config.GetValue<string>("GPT:organizationId"));
        try
        {
            var results = await chatOpenAI.SendChatMessage(command.Data.Options.First().Value.ToString());

            foreach (var item in results)
            {
                Console.WriteLine($"{item.Role}: {item.Content}");
                await command.FollowupAsync($"{item.Content}");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

    }

    
}