using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Model> Models { get; set; }
  }
  public class Model
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public Model(string name, string settings, string description = "")
    {
      Name = name;
      Settings = settings;
      Description = description;
    }
  }
}