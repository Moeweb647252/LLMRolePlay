using Microsoft.EntityFrameworkCore;
using System.IO.Pipelines;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.LogTo(Console.WriteLine, LogLevel.None);
      base.OnConfiguring(optionsBuilder);
    }

  }
}