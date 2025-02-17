using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Models
{
  public class DbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Preset> Presets { get; set; }
    public DbSet<Provider> Providers { get; set; }

    string DbPath { get; set; }
    public DbContext()
    {
      var directory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/llmroleplay");
      Directory.CreateDirectory(directory);
      DbPath = Path.Join(directory, "/llmroleplay.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
  }
}