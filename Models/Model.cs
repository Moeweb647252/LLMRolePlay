using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public class Model
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Settings { get; set; }
    [ForeignKey("User")]
    public required int UserId { get; set; }
    public required User User { get; set; }
    [ForeignKey("Provider")]
    public required int ProviderId { get; set; }
    public required Provider Provider { get; set; }
  }
}