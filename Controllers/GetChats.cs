using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getChats")]
    [AllowAnonymous]
    public async Task<ApiResponse> GetChats()
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return ApiResponse.TokenError();

      return ApiResponse.Success(new
      {
        chats = user.Chats.Select(chat => new GetChatResult(
          chat.Id,
          chat.Name,
          chat.Settings,
          chat.Model,
          chat.Character,
          chat.Preset,
          chat.Messages.Count
        ))
      });
    }
    public class GetChatResult(uint id, string name, string settings, Model model, Character character, Preset preset, int messageCount)
    {
      public uint Id { get; set; } = id;
      public string Name { get; set; } = name;
      public string Settings { get; set; } = settings;
      public Model Model { get; set; } = model;
      public Character Character { get; set; } = character;
      public Preset Preset { get; set; } = preset;
      public int MessageCount { get; set; } = messageCount;
    }
  }
}
