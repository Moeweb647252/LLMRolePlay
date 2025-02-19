using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Chat> Chats { get; set; }
  }
  public class Chat
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Settings { get; set; }
    public Model Model { get; private set; }
    public Character Character { get; private set; }
    public Preset Preset { get; private set; }
    public List<Message> Messages { get; private set; } = new List<Message>();
    public Chat()
    {

    }
    public Chat(string name, string settings, Model model, Character character, Preset preset)
    {
      Name = name;
      Settings = settings;
      Model = model;
      Character = character;
      Preset = preset;
    }
    /// <summary>
    /// 不会自动绑定到用户。Do not bind to user automically.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    /// <param name="modelId"></param>
    /// <param name="characterId"></param>
    /// <param name="presetId"></param>
    /// <returns></returns>
    public static async Task<Chat?> CreateChat(DBContext db, string name, string settings, uint modelId, uint characterId, uint presetId)
    {
      Model? model = await Model.GetModelById(db, modelId);
      Character? character = await Character.GetCharacterById(db, characterId);
      Preset? preset = await Preset.GetPresetById(db, presetId);
      if (model == null || character == null || preset == null) return null;
      Chat chat = new Chat(name, settings, model, character, preset);
      await db.Chats.AddAsync(chat);
      await db.SaveChangesAsync();
      return chat;
    }
    public static async Task<Chat?> GetChatById(DBContext db,uint chatId)
    {
      return await db.Chats.FindAsync(chatId);
    }
  }
}