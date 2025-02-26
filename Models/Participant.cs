using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Participant> Participants { get; set; }
  }

  public class Participant
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public Model Model { get; set; } = null!;
    [ForeignKey("Model")]
    public uint ModelId { get; set; }
    public Character Character { get; set; } = null!;
    [ForeignKey("Character")]
    public uint CharacterId { get; set; }
    public Preset Preset { get; set; } = null!;
    [ForeignKey("Preset")]
    public uint PresetId { get; set; }
    public Chat Chat { get; set; } = null!;
    [ForeignKey("Chat")]
    public uint ChatId { get; set; }

    public Template Template { get; set; } = null!;
    [ForeignKey("Template")]
    public uint TemplateId { get; set; }

    public Participant(uint modelId, uint characterId, uint presetId, uint chatId, uint templateId)
    {
      ModelId = modelId;
      CharacterId = characterId;
      PresetId = presetId;
      ChatId = chatId;
      TemplateId = templateId;
    }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}