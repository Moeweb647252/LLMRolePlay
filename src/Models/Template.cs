using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Template> Templates { get; set; }
  }

  public class Template
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public uint UserId { get; set; }
    public string? Settings { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }

}