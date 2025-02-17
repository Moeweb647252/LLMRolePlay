namespace LLMRolePlay.Models
{
  public class Character
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Settings { get; set; }
    public required int UserId { get; set; }
    public required User User { get; set; }
  }
}