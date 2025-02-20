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
    public string Settings { get; set; }
    public string Description { get; set; }
    public Model(string name, string settings, string description = "")
    {
      Name = name;
      Settings = settings;
      Description = description;
    }
    public static async Task<Model> CreateModel(DBContext db, string name, string settings, string description)
    {
      Model model = new Model(name, settings, description);
      await db.Models.AddAsync(model);
      await db.SaveChangesAsync();
      return model;
    }
    public static async Task<Model?> GetModelById(DBContext db,uint id)
    {
      return await db.Models.FindAsync(id);
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