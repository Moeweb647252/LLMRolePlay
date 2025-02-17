namespace LLMRolePlay.Models
{
  public class Preset
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Settings { get; set; }
    public User? User { get; set; }
  }
}