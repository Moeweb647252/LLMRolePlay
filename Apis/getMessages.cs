using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Apis
{
  public partial class Api
  {
    public async Task<ApiResponse> GetMessages(HttpContext req, int chatId)
    {
      var user = await GetUser(req);
      var chat = await dbContext.Chats.SingleAsync(c => c.Id == chatId && c.UserId == user.Id);
      var messages = await dbContext.Messages
        .Where(m => m.ChatId == chatId)
        .OrderBy(m => m.CreatedAt)
        .ToListAsync();
      return new ApiResponse
      {
        code = 200,
        message = "Messages retrieved successfully",
        data = messages
      };
    }
  }
}