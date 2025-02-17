namespace LLMRolePlay.Models
{
  public class Model
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Settings { get; set; }
    public required User User { get; set; }
    public required Provider Provider { get; set; }
  }
}