using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<User> Users { get; set; }
  }
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增id
    public uint Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Token { get; private set; }
    public Group Group { get; set; }
    public List<Chat> Chats { get; private set; } = new List<Chat>();
    public List<Character> Characters { get; private set; } = new List<Character>();
    public List<Preset> Presets { get; private set; } = new List<Preset>();
    public List<Provider> Providers { get; private set; } = new List<Provider>();

    public User(string userName, string email, string password, Group group)
    {
      UserName = userName;
      Email = email;
      Password = password;
      Group = group;
    }
    public static async Task CreateAdmin(DBContext db, string userName, string email, string password)
    {
      await db.Users.AddAsync(new User(userName, email, password, Group.Admin));
      await db.SaveChangesAsync();
    }
    public static async Task CreateUser(DBContext db, string userName, string email, string password)
    {
      await db.Users.AddAsync(new User(userName, email, password, Group.User));
      await db.SaveChangesAsync();
    }
    public static async Task<User?> GetUserById(DBContext db, uint id)
    {
      return await db.Users.FindAsync(id);
    }
    public async Task SaveChanges(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
      await db.SaveChangesAsync();
    }
    public async Task Delete(DBContext db)
    {
      db.Users.Remove(this);
      await db.SaveChangesAsync();
    }
    public async Task<string> UpdateToken(DBContext db)
    {
      Token = GenerateToken();
      await SaveChanges(db);
      return Token;
    }
    private string GenerateToken()
    {
      return Guid.NewGuid().ToString("D");
    }
  }
  public enum Group : byte
  {
    Admin = 0, User = 1
  }
}