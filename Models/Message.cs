namespace LLMRolePlay.Models
{
  public class Message
  {
    public int Id { get; set; }
    public required string Role { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required int UserId { get; set; }
    public required int ChatId { get; set; }
  }
}