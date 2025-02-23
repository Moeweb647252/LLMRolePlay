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
    public string Role { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Chat Chat { get; set; } = null!;
    [ForeignKey("Chat")]
    public uint ChatId { get; set; }
    public Message(string role, string content, uint chatId)
    {
      Role = role;
      Content = content;
    }
    public static async Task<Message> CreateMessage(DBContext db, string role, string content, uint chatId)
    {
      Message message = new Message(role, content, chatId);
      await db.Messages.AddAsync(message);
      await db.SaveChangesAsync();
      return message;
    }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}