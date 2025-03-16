using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Provider> Providers { get; set; }
  }
  public class Provider
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; }
    public required string Settings { get; set; }
    public string? Description { get; set; }
    public ICollection<Model> Models { get; } = new List<Model>();
    public uint UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
    public async Task Delete(DBContext db)
    {
      db.Providers.Remove(this);
      await db.SaveChangesAsync();
    }
  }
}