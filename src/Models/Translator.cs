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
    public required string Name { get; set; }
    public string? Description { get; set; } = null;
    public required string Settings { get; set; }
    public Model Model { get; set; } = null!;
    [ForeignKey("Model")]
    public uint ModelId { get; set; }
    public string PresetIds { get; set; } = string.Empty;
    public Template Template { get; set; } = null!;
    [ForeignKey("Template")]
    public uint TemplateId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required uint UserId { get; set; }

    public void MarkAsModified(DBContext db)
    {
      db.Entry(this).State = EntityState.Modified;
    }

    public async Task<String> MakeSystemPrompt(DBContext db, string targetLanguage)
    {
      string ret = Template.Content;

      // Process presets if any
      if (!string.IsNullOrEmpty(PresetIds))
      {
        var presetIds = PresetIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(uint.Parse);
        var presets = await db.Presets.Where(p => presetIds.Contains(p.Id)).ToListAsync();

        // Format presets
        string presetsContent = string.Join("\n",
          presets.Select(preset =>
            string.Join("\n",
              System.Text.Json.JsonSerializer.Deserialize<List<ContentItem>>(
                preset.Content ?? ""
              )?.Select(s => $"{s.Key}: {s.Value}") ?? []
            )
          )
        );

        // Replace placeholder or append
        if (ret.Contains("{{ preset }}"))
        {
          ret = ret.Replace("{{ preset }}", presetsContent);
        }
        else if (!string.IsNullOrEmpty(presetsContent))
        {
          ret = ret + "\nPreset:\n" + presetsContent;
        }
      }

      // Add description if available
      if (!string.IsNullOrEmpty(Description))
      {
        ret += $"\nTranslator Description: {Description}\n";
      }

      ret += "你是一个翻译器，你的任务是将用户的输入翻译成目标语言。\n";
      ret += "你需要严格遵循以下格式进行翻译，决不允许出现不符合格式的内容、提示:\n";
      ret += $"目标语言: {targetLanguage}\n";
      ret += "需翻译的内容格式如下:\n";
      ret += @" {
        ""total_chunks"": 2,
        ""chunks"": [{
          ""chunk_id"": 1,
          ""content"": ""Hello, world!""
        }, {
          ""chunk_id"": 2,
          ""content"": ""Hello, world!""
        }]
      }
      ";
      ret += "请将内容翻译成目标语言，并返回格式如下:\n";
      ret += @"
      1: 你好，世界！
      2: 你好，世界！

      END: Total Chunks: 2
      ";
      return ret;
    }
  }
}