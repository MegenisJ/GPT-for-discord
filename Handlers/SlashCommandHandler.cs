using Discord.WebSocket;
using GPT_for_discord.Services.Interfaces;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using static Program;
using Discord;
using GPT_for_discord.Models;

namespace GPT_for_discord.Handlers;

public class SlashCommandHandler
{
    private readonly IOpenAIProxy _chatOpenAI;
    private readonly List<GPTCommand> _gptCommands;
    public SlashCommandHandler(IOpenAIProxy chatOpenAI, List<GPTCommand>  gptCommands)
    {
        _chatOpenAI = chatOpenAI;
        _gptCommands = gptCommands;
    }
    public async Task Handler(SocketSlashCommand command)
    {
        _ = Task.Run(async () =>
        {
            if (command.CommandName == SlashCommands.commandbuilder.ToString())
            {
                _ = await CommandBuilderResponse(command);
                return Task.CompletedTask;
            }
            var response = Array.Empty<ChatCompletionMessage>();
            await command.RespondAsync(command.User.Username + " asked : " + command.Data.Options.First().Value.ToString());
            var commandMessage = command.Data.Options.First().Value.ToString();
            if (command.CommandName == SlashCommands.gpt.ToString())
            {
                response = await AiResponseHandler(commandMessage, "", "");

            }
            foreach (var gptCommand in _gptCommands)
            {
                if (command.CommandName == gptCommand.name)
                {
                    response = await AiResponseHandler(commandMessage, gptCommand.AiPrefix, gptCommand.AiSuffix);
                }
            }

            foreach (var item in response)
            {
                int chunkSize = 1500;
                int stringLength = item.Content.Length;
                for (int i = 0; i < stringLength; i += chunkSize)
                {
                    if (i + chunkSize > stringLength) chunkSize = stringLength - i;
                    Console.WriteLine(item.Content.Substring(i, chunkSize));
                    await command.FollowupAsync($"```{item.Content.Substring(i, chunkSize)}```");
                }
            }
            return Task.CompletedTask;
        });
       
        
    }
    public async Task<ChatCompletionMessage[]> AiResponseHandler(string command, string prefix, string suffix)
    {
        try
        {
            var results = await _chatOpenAI.SendChatMessage(prefix + command + suffix);

            return results;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        return Array.Empty<ChatCompletionMessage>();
    }

    private async Task<bool> CommandBuilderResponse(SocketSlashCommand command)
    {

        var mb = new ModalBuilder()
            .WithTitle("Bot builder")
            .WithCustomId("bot_builder")
            .AddTextInput("commandname", "ai_name", placeholder: "")
            .AddTextInput("prefix", "ai_prefix", placeholder: "")
            .AddTextInput("suffix", "ai_suffix", placeholder: "");

        await command.RespondWithModalAsync(mb.Build());

        return true;
    }
}
