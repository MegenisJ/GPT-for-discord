namespace GPT_for_discord.Models
{
    public class GPTCommand : Command
    {
        public string name { get; set; }
        public string AiPrefix { get; set; }
        public string AiSuffix { get; set; }
    }
}
