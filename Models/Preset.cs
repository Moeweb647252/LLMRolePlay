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
    public string Name { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public uint UserId { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Preset(string name, string settings, uint userId, string description = "", bool isPublic = false)
    {
      Name = name;
      Settings = settings;
      Description = description;
      UserId = userId;
      IsPublic = isPublic;
    }
    /// <summary>
    /// 不会自动绑定到用户。Do not bind to user automically.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    /// <param name="description"></param>
    /// <param name="userId"></param>
    /// <param name="isPublic"></param>
    /// <returns></returns>
    public static async Task<Preset> CreatePreset(DBContext db, string name, string settings, string description, uint userId, bool isPublic = false)
    {
      Preset preset = new Preset(name, settings, userId, description, isPublic);
      await db.Presets.AddAsync(preset);
      await db.SaveChangesAsync();
      return preset;
    }
    public static async Task<Preset?> GetPresetById(DBContext db, uint id)
    {
      return await db.Presets.FindAsync(id);
    }
    public async Task Delete(DBContext db)
    {
      db.Presets.Remove(this);
      await db.SaveChangesAsync();
    }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}