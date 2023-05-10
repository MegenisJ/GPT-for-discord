using Discord;
using GPT_for_discord.Models;

namespace GPT_for_discord;

public class Setup
{
    public List<Command> InitialCommands = new() {
        new Command()
        {
            builder = new SlashCommandBuilder()
            {
                Name = SlashCommands.gpt.ToString(),
                Description = "ask me anything!",
                Options = new List<SlashCommandOptionBuilder> { new SlashCommandOptionBuilder() {
                    Name = "message",
                    Type = ApplicationCommandOptionType.String,
                    IsRequired = true,
                    Description = "message send to mej ai"
                } }
            }
        },
        new Command()
        {
            builder = new SlashCommandBuilder()
            {
                Name = SlashCommands.gpt.ToString(),
                Description = "ask me anything!"
            }
        }
    };
}

