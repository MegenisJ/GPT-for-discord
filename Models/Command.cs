using Discord;
using Discord.WebSocket;

namespace GPT_for_discord.Models;

public class Command
{
    public SlashCommandBuilder builder { get; set; }       
    public async Task Create (DiscordSocketClient discordClient)
    {
       await discordClient.CreateGlobalApplicationCommandAsync(builder.Build());
    }
}
