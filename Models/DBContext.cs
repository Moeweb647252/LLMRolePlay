using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }
  }
}