using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public class Chat
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Settings { get; set; }
    [ForeignKey("User")]
    public required int UserId { get; set; }
    public required User User { get; set; }
    [ForeignKey("Model")]
    public required int ModelId { get; set; }
    public required Model Model { get; set; }
    [ForeignKey("Character")]
    public required int CharacterId { get; set; }
    public required Character Character { get; set; }
    [ForeignKey("Preset")]
    public required int PresetId { get; set; }
    public required Preset Preset { get; set; }

  }
}