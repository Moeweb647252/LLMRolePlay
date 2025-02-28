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
    public string? Settings { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public uint UserId { get; set; }

    public uint? Avatar { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Character(string name, string content, uint userId, string description = "", bool isPublic = false, uint? avatar = null)
    {
      Name = name;
      Content = content;
      UserId = userId;
      Description = description;
      IsPublic = isPublic;
      Avatar = avatar;
    }
    public static async Task<Character> CreateCharacter(DBContext db, string name, string settings, string description, uint userId, bool isPublic = false, uint? avatar = null)
    {
      Character character = new Character(name, settings, userId, description, isPublic, avatar);
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