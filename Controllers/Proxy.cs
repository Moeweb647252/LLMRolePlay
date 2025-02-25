using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [Route("proxy/{token}/{modelId}/{*path}")]
    [AllowAnonymous]
    public async Task Proxy(HttpContext ctx, string token, uint modelId, string? path)
    {
      User? user = await Models.User.GetUserByToken(_dBContext, token);
      if (user == null) return;
      Model? model = await Model.GetModelById(_dBContext, modelId);
      if (model == null) return;
      Provider provider = model.Provider;
      if (provider == null) return;
      if (provider.UserId != user.Id && !model.IsPublic) return;
      string baseUrl = provider.BaseUrl;
      string url = baseUrl + "/" + path;
      HttpClient client = new HttpClient();
      var request = new HttpRequestMessage(HttpMethod.Parse(ctx.Request.Method), url);
      request.Headers.Add("Authorization", "Bearer " + provider.ApiKey);
      HttpResponseMessage response = await client.SendAsync(request);
      if (response.IsSuccessStatusCode)
      {
        var stream = await response.Content.ReadAsStreamAsync();
        await stream.CopyToAsync(ctx.Response.Body);
      }
    }
  }
}