using System.Text.Json.Serialization;

namespace LLMRolePlay.Models
{
  public class ChatMessage
  {
    public required string content { get; set; }
    public required string role { get; set; }
  }
}