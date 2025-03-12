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
    public string Name { get; set; }
    public string ModelName { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public Provider Provider { get; set; } = null!;
    [ForeignKey("Provider")]
    public uint ProviderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Model(string name, string modelName, string settings, uint providerId, bool isPublic, string description = "")
    {
      Name = name;
      Settings = settings;
      Description = description;
      ModelName = modelName;
      ProviderId = providerId;
      IsPublic = isPublic;
    }
    public static async Task<Model> CreateModel(DBContext db, string name, string settings, string description, string modelName, uint providerId, bool isPublic)
    {
      Model model = new Model(name, modelName, settings, providerId, isPublic, description);
      await db.Models.AddAsync(model);
      await db.SaveChangesAsync();
      return model;
    }
    public static async Task<Model?> GetModelById(DBContext db, uint id)
    {
      return await db.Models.Include(m => m.Provider).FirstOrDefaultAsync(m => m.Id == id);
    }
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