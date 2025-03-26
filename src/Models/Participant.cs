using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Participant> Participants { get; set; }
  }

  public class ContentItem
  {
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
  }

  public class Participant
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Settings { get; set; }
    public Model Model { get; set; } = null!;
    [ForeignKey("Model")]
    public uint ModelId { get; set; }
    public Character Character { get; set; } = null!;
    [ForeignKey("Character")]
    public uint CharacterId { get; set; }

    // 将单一PresetId改为存储逗号分隔的PresetIds字符串
    public string PresetIds { get; set; } = string.Empty;

    // 移除单一Preset导航属性

    public Chat Chat { get; set; } = null!;
    [ForeignKey("Chat")]
    public uint ChatId { get; set; }

    public Template Template { get; set; } = null!;
    [ForeignKey("Template")]
    public uint TemplateId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 添加一个新的构造函数，接受多个PresetId
    public Participant(uint modelId, uint characterId, string presetIds, uint chatId, uint templateId, string name, string settings)
    {
      ModelId = modelId;
      CharacterId = characterId;
      PresetIds = string.Join(",", presetIds);
      ChatId = chatId;
      TemplateId = templateId;
      Name = name;
      Settings = settings;
    }

    public Participant(uint modelId, uint characterId, IEnumerable<uint> presetIds, uint chatId, uint templateId, string name, string settings)
    {
      ModelId = modelId;
      CharacterId = characterId;
      PresetIds = string.Join(",", presetIds);
      ChatId = chatId;
      TemplateId = templateId;
      Name = name;
      Settings = settings;
    }

    // 辅助方法，获取PresetId列表
    public IEnumerable<uint> GetPresetIdList()
    {
      if (string.IsNullOrEmpty(PresetIds))
        return new List<uint>();

      return PresetIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(uint.Parse);
    }

    public async Task<IEnumerable<Preset>> GetPresetList(DBContext db)
    {
      if (string.IsNullOrEmpty(PresetIds))
        return new List<Preset>();

      var ids = PresetIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(uint.Parse);
      var verified = new List<Preset>();
      foreach (var id in ids)
      {
        var preset = await db.Presets.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (preset != null)
        {
          verified.Add(preset);
        }
      }
      if (verified.Count != ids.Count())
      {
        PresetIds = string.Join(",", verified.Select(p => p.Id));
        MarkAsModified(db);
        await db.SaveChangesAsync();
      }
      return verified;
    }

    // 添加一个PresetId
    public void AddPresetId(uint presetId)
    {
      var idList = GetPresetIdList().ToList();
      if (!idList.Contains(presetId))
      {
        idList.Add(presetId);
        PresetIds = string.Join(",", idList);
      }
    }

    // 移除一个PresetId
    public bool RemovePresetId(uint presetId)
    {
      var idList = GetPresetIdList().ToList();
      bool result = idList.Remove(presetId);
      if (result)
      {
        PresetIds = string.Join(",", idList);
      }
      return result;
    }

    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }

    public async Task<string> MakeSystemPrompt(DBContext db)
    {

      string ret = Template.Content;
      string character = "";
      character += "\nYour Character:\n";
      character += $"姓名: {Character.Name}\n";
      character += "描述:" + Character.Description + "\n";
      character += string.Join("\n",
          JsonSerializer.Deserialize<List<ContentItem>>(Character.Content)?
          .Select(s => $"{s.Key}: {s.Value}") ?? []
        );
      if (ret.Contains("{{ character }}"))
      {
        ret = ret.Replace("{{ character }}", character);
      }
      else
      {
        ret += character;
      }
      string preset = string.Join("\n", (await GetPresetList(db)).Select(preset =>
       string.Join("\n",
          JsonSerializer.Deserialize<List<ContentItem>>(
            preset.Content ?? ""
          )?
          .Select(s => $"{s.Key}: {s.Value}") ?? []
        )
      ));
      if (ret.Contains("{{ preset }}"))
      {
        ret = ret.Replace("{{ preset }}", preset);
      }
      else
      {
        ret = ret + "\nPreset:\n" + preset;
      }
      var chat = await db.Chats.Include(c => c.Participants).ThenInclude(p => p.Character).Where(c => c.Id == ChatId).FirstAsync();
      if (chat.Description != null) ret += $"\nChatroom Description: {chat.Description}\n";
      ret += "\nOther participants in chat: \n";
      foreach (var participant in Chat.Participants)
      {
        if (participant.Id == Id)
        {
          continue;
        }
        ret += $"姓名: {participant.Name}\n";
        ret += "描述:" + participant.Character.Description + "\n";
        ret += string.Join("\n",
          JsonSerializer.Deserialize<List<ContentItem>>(participant.Character.Content)?
          .Select(s => $"{s.Key}: {s.Value}") ?? []
        );
        ret += "\n\n";
      }
      return ret;
    }
  }
}