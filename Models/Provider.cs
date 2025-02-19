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
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public List<Model> Models { get; private set; } = new List<Model>();
    public Provider(string name, string type, string settings, string description = "")
    {
      Name = name;
      Type = type;
      Settings = settings;
      Description = description;
    }
    /// <summary>
    /// 不会自动绑定到用户。Do not bind to user automically.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="settings"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static async Task<Provider> CreateProvider(DBContext db, string name, string type, string settings, string description)
    {
      Provider provider = new Provider(name, type, settings, description);
      await db.Providers.AddAsync(provider);
      await db.SaveChangesAsync();
      return provider;
    }
    public static async Task<Provider?> GetProviderById(DBContext db, uint id)
    {
      return await db.Providers.FindAsync(id);
    }
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