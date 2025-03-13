using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLMRolePlay.Models
{
  public partial class DBContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public DbSet<Translator> Translators { get; set; }
  }

  public class Translator
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }
    public string Name { get; set; }
    public string Settings { get; set; }
    public Model Model { get; set; } = null!;
    [ForeignKey("Model")]
    public uint ModelId { get; set; }
    public string PresetIds { get; set; } = string.Empty;
    public Template Template { get; set; } = null!;
    [ForeignKey("Template")]
    public uint TemplateId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 添加一个新的构造函数，接受多个PresetId
    public Translator(uint modelId, string presetIds, uint templateId, string name, string settings)
    {
      ModelId = modelId;
      PresetIds = string.Join(",", presetIds);
      TemplateId = templateId;
      Name = name;
      Settings = settings;
    }

    public Translator(uint modelId, IEnumerable<uint> presetIds, uint templateId, string name, string settings)
    {
      ModelId = modelId;
      PresetIds = string.Join(",", presetIds);
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
  }
}