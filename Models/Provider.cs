using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Provider> Providers { get; set; }
  }
  public class Provider
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Settings { get; set; }
    public string Description { get; set; }
    public List<Model> Models { get; private set; } = new List<Model>();
    public Provider(string name, string type, string settings, string description = "")
    {
      Name = name;
      Type = type;
      Settings = settings;
      Description = description;
    }
  }
}