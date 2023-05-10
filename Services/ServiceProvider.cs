using Discord.WebSocket;
using GPT_for_discord.Handlers;
using GPT_for_discord.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GPT_for_discord.Services;

public class ServiceProvider
{
    public static IServiceProvider CreateServices()
    {
        var config = new DiscordSocketConfig()
        {
            UseInteractionSnowflakeDate = false,

        };

        var collection = new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<IOpenAIProxy, OpenAIProxy>()
            .AddSingleton<SlashCommandHandler>();

        return collection.BuildServiceProvider();

    }
}


