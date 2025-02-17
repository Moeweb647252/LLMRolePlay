using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public class Message
  {
    public int Id { get; set; }
    public required string Role { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [ForeignKey("User")]
    public required int UserId { get; set; }
    public required User User { get; set; }
    [ForeignKey("Chat")]
    public required int ChatId { get; set; }
    public required Chat Chat { get; set; }
  }
}