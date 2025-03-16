using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Chat> Chats { get; set; }
  }

  public class ChatSettings
  {
    public string? NameOfUser { get; set; }
    public uint? currentParticipantId { get; set; }
  }

  public class Chat
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Settings { get; set; }

    public ICollection<Message> Messages { get; } = new List<Message>();
    public ICollection<Participant> Participants { get; } = new List<Participant>();
    public uint UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}