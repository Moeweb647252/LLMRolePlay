using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Preset> Presets { get; set; }
  }

  public class Preset
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增id
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Settings { get; set; }
    public required string Content { get; set; }
    public string? Description { get; set; }
    public uint UserId { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}