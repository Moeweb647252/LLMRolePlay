using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Character> Characters { get; set; }
  }
  public class Character
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public uint UserId { get; set; }
    public Character(string name, string settings, uint userId, string description = "")
    {
      Name = name;
      Description = description;
      Settings = settings;
      UserId = userId;
    }
    /// <summary>
    /// 不会自动绑定到用户。Do not bind to user automically.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    /// <param name="description"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static async Task<Character> CreateCharacter(DBContext db, string name, string settings, string description, uint userId)
    {
      Character character = new Character(name, settings, userId, description);
      await db.Characters.AddAsync(character);
      await db.SaveChangesAsync();
      return character;
    }
    public static async Task<Character?> GetCharacterById(DBContext db, uint id)
    {
      return await db.Characters.FindAsync(id);
    }
    public async Task Delete(DBContext db)
    {
      db.Characters.Remove(this);
      await db.SaveChangesAsync();
    }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}