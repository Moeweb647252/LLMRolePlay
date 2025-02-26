using LLMRolePlay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLMRolePlay.Controllers
{
  public partial class API : ControllerBase
  {
    public class DeleteUserRequest
    {
      public required uint userId { get; set; }
    }
  }

  public partial class API : ControllerBase
  {
    [HttpPost("deleteUser")]
    [AllowAnonymous]
    public async Task<ApiResponse> DeleteUser([FromBody] DeleteUserRequest data)
    {
      string? token = Request.Headers["token"];
      if (token == null) return ApiResponse.TokenError();

      User? currentUser = await Models.User.GetUserByToken(_dBContext, token);
      if (currentUser == null) return ApiResponse.TokenError();

      // Only admins can delete users
      if (currentUser.Group != Group.Admin) return ApiResponse.MessageOnly(403, "Insufficient permissions");

      User? userToDelete = await Models.User.GetUserById(_dBContext, data.userId);
      if (userToDelete == null) return ApiResponse.MessageOnly(404, "User not found");

      // Prevent admins from deleting themselves
      if (userToDelete.Id == currentUser.Id) return ApiResponse.MessageOnly(400, "Cannot delete your own account");

      await userToDelete.Delete(_dBContext);
      return ApiResponse.Success();
    }
  }
}