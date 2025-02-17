namespace LLMRolePlay.Models
{
  public class Chat
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Settings { get; set; }
    public required int UserId { get; set; }
    public required int ModelId { get; set; }
    public required int CharacterId { get; set; }
    public required int PresetId { get; set; }

  }
}