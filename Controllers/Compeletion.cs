using LLMRolePlay.Models;
using LLMRolePlay.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LLMRolePlay.Controllers {
    
    public class CompeletionRequest {
        public required uint modelId;
        public required uint chatId;
    }
    public partial class API : ControllerBase {
        [HttpPost("compeletion")]
        [AllowAnonymous]
        public async Task Compeletion(HttpContext ctx, [FromBody] CompeletionRequest data) {
        string? token = Request.Headers["token"];
            if (token == null) return;
            User? user = await Models.User.GetUserByToken(_dBContext, token);
            if (user == null) return;
            Model? model = await Model.GetModelById(_dBContext, data.modelId);
            if (model == null) return;
            if (model.Provider.UserId != user.Id && !model.IsPublic) return;
            Chat? chat = await _dBContext.Chats.Include(c=>c.Messages).FirstOrDefaultAsync(c=>c.Id == data.chatId);
            if (chat == null) return;
            if (chat.UserId != user.Id) return;
            if (model.Provider.Type == "openai") {
                var settings = JsonConvert.DeserializeObject<OpenAISettings>(model.Provider.Settings);
            }
        }
    }
}