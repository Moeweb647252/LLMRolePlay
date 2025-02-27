using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

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
    [JsonPropertyName("username")]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Token { get; private set; }
    public Group Group { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User(string userName, string email, string password, Group group)
    {
      UserName = userName;
      Email = email;
      Password = password;
      Group = group;
    }
    public static async Task<User> Create(DBContext db, string userName, string email, string password, Group group)
    {
      User user = new User(userName, email, password, group);
      await db.Users.AddAsync(user);
      await db.SaveChangesAsync();
      return user;
    }
    public static async Task<User> CreateAdmin(DBContext db, string userName, string email, string password)
    {
      User user = new User(userName, email, password, Group.Admin);
      await db.Users.AddAsync(user);
      await db.SaveChangesAsync();
      return user;
    }
    public static async Task<User> CreateUser(DBContext db, string userName, string email, string password, Group group = Group.User)
    {
      User user = new User(userName, email, password, group);
      await db.Users.AddAsync(user);
      await db.SaveChangesAsync();
      return user;
    }
    public static async Task<bool> Authenticate(DBContext db, string token, Group lowestGroup)
    {
      User? user = await GetUserByToken(db, token);
      if (user == null) return false;
      if (user.Group >= lowestGroup)
      {
        return true;
      }
      return false;
    }
    public static async Task<User?> GetUserById(DBContext db, uint id)
    {
      return await db.Users.FindAsync(id);
    }

    public static async Task<User?> GetUserByToken(DBContext db, string token)
    {
      List<User> users = await db.Users.Where(user => user.Token == token).ToListAsync();
      if (users.Count == 0) return null;
      return users[0];
    }

    public static async Task<User?> GetUserByRequest(DBContext db, HttpContext request)
    {
      string? token = request.Request.Headers["token"];
      if (token == null) return null;
      return await GetUserByToken(db, token);
    }

    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
    public async Task Delete(DBContext db)
    {
      db.Users.Remove(this);
      await db.SaveChangesAsync();
    }
    public async Task<string> UpdateToken(DBContext db)
    {
      Token = GenerateToken();
      MarkAsModified(db);
      await db.SaveChangesAsync();
      return Token;
    }
    private string GenerateToken()
    {
      return Guid.NewGuid().ToString("D");
    }
  }
  [Flags]
  public enum Group : ushort
  {
    User = 0b_0000_0001,
    Admin = 0b_0000_0010
  }
}