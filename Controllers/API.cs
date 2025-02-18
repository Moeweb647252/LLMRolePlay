using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  [ApiController]
  [Route("api")]
  public partial class API : ControllerBase
  {
    private readonly DBContext _dBContext;
    public API(DBContext dBContext)
    {
      _dBContext = dBContext;
    }
  }
}
