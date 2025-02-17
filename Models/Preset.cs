using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public class Preset
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Settings { get; set; }
    [ForeignKey("User")]
    public required int UserId { get; set; }
    public required User User { get; set; }

  }
}