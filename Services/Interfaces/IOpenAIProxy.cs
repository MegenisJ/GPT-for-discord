using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace GPT_for_discord.Services.Interfaces;
public interface IOpenAIProxy
{
    Task<ChatCompletionMessage[]> SendChatMessage(string message);
}
