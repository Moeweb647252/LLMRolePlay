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
    public Character(string name, string settings, string description = "")
    {
      Name = name;
      Description = description;
      Settings = settings;
    }
  }
}