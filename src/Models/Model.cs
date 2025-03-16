using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Model> Models { get; set; }
  }
  public class Model
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Name { get; set; }
    public required string ModelName { get; set; }
    public required string Settings { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public Provider Provider { get; set; } = null!;
    [ForeignKey("Provider")]
    public uint ProviderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public async Task Delete(DBContext db)
    {
      db.Models.Remove(this);
      await db.SaveChangesAsync();
    }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}