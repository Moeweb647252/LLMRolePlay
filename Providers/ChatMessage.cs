using System.Text.Json.Serialization;

namespace LLMRolePlay.Models
{
  public class ChatMessage
  {
    [JsonPropertyName("content")]
    public required string Content { get; set; }
    [JsonPropertyName("role")]
    public required string Role { get; set; }
  }
}