using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Message> Messages { get; set; }
  }
  public class Message
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong Id { get; set; }
    public required string Role { get; set; }
    public required string Content { get; set; }
    public uint? Tokens { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public uint? ParticipantId { get; set; }
    public uint? FileId { get; set; }
    public string? FileType { get; set; }
    public Chat Chat { get; set; } = null!;

    [ForeignKey("Chat")]
    public required uint ChatId { get; set; }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}