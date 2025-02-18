using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Preset> Presets { get; set; }
  }
  public class Preset
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//×ÔÔöid
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public Preset(string name,string settings,string description="")
    {
      Name = name;
      Settings = settings;
      Description = description;
    }
  }
}