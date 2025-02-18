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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//×ÔÔöid
    public uint Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Token { get; private set; }
    public List<Chat> Chats { get; private set; } = new List<Chat>();
    public List<Character> Characters { get; private set; } = new List<Character>();
    public List<Preset> Presets { get; private set; } = new List<Preset>();
    public List<Provider> Providers { get; private set; } = new List<Provider>();

    public User(string userName, string email, string password)
    {
      UserName = userName;
      Email = email;
      Password = password;
    }
    public static async void CreateUser(string userName, string email, string password)
    {
      using (var db = new DBContext())
      {
        await db.Users.AddAsync(new User(userName, email, password));
      }
    }
    public static async Task<User?> GetUserById(uint id)
    {
      using (var db = new DBContext())
      {
        return await db.Users.FindAsync(id);
      }
    }
    public async Task SaveChanges()
    {
      using (var db = new DBContext())
      {
        db.Entry(this).State = EntityState.Modified;
        await db.SaveChangesAsync();
      }
    }
    public async Task Delete()
    {
      using (var db = new DBContext())
      {
        db.Users.Remove(this);
        await db.SaveChangesAsync();
      }
    }
    public async Task<string> UpdateToken()
    {
      using (var db = new DBContext())
      {
        Token = GenerateToken();
        await SaveChanges();
        return Token;
      }
    }
    private string GenerateToken()
    {
      return Guid.NewGuid().ToString("D");
    }
  }
}