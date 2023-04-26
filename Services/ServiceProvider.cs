using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPT_for_discord.Services;

public class ServiceProvider
{
    public static IServiceProvider CreateServices()
    {

        var config = new DiscordSocketConfig()
        {
            UseInteractionSnowflakeDate = false
        };

        var collection = new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<DiscordSocketClient>();

        return collection.BuildServiceProvider();
    }

}


