using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    [HttpPost("getUsers")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse> GetUsers()
    {

      return ApiResponse.Success(new
      {
        users = await _dBContext.Users.ToListAsync()
      });
    }
  }
}