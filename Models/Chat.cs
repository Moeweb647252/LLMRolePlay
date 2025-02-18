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
  }
}