using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<File> Files { get; set; }
  }

  public class File
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public byte[] Data { get; set; }
    public uint UserId { get; set; }

    public File(byte[] data, uint userId)
    {
      Data = data;
      UserId = userId;
    }
  }

}