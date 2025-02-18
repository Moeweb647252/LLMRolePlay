using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public static string DbPath 
    {
      get
      {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LLMRolePlay");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "LLMRolePlay.db");
      }
    }
    public DBContext() { }
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
  }
}