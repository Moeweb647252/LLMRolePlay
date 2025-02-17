namespace LLMRolePlay.Models
{
  public class Provider
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Type { get; set; }
    public required string Settings { get; set; }
    public User? User { get; set; }
  }
}