using LLMRolePlay.Models;

namespace LLMRolePlay.Providers {
    public class OpenAISettings {
        public double? temperature;
        public ulong? max_tokens;
        public double? top_p;
    }

    public class OpenAI {
        public required string apiKey;
        public required string baseUrl;

        public OpenAI(string apiKey, string baseUrl) {
            this.apiKey = apiKey;
            this.baseUrl = baseUrl;
        }

        public async Task Compeletion(string model, IEnumerable<ChatMessage> messages, OpenAISettings settings) {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            HttpRequest request = 
        }
    }
}