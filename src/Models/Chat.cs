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
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Settings { get; set; }

    public ICollection<Message> Messages { get; } = new List<Message>();
    public ICollection<Participant> Participants { get; } = new List<Participant>();
    public uint UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Chat(string name, string? description, string settings, uint userId)
    {
      Name = name;
      Description = description;
      Settings = settings;
      UserId = userId;
    }
    /// <summary>
    /// 不会自动绑定到用户。Do not bind to user automically.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static async Task<Chat?> CreateChat(DBContext db, string name, string? description, string settings, uint userId)
    {
      Chat chat = new Chat(name, description, settings, userId);
      await db.Chats.AddAsync(chat);
      await db.SaveChangesAsync();
      return chat;
    }
    public static async Task<Chat?> GetChatById(DBContext db, uint chatId)
    {
      return await db.Chats.FindAsync(chatId);
    }
    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }
  }
}