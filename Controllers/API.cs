using LLMRolePlay.Models;
using Microsoft.AspNetCore.Mvc;

namespace LLMRolePlay.Controllers
{
  /// <summary>
  /// 公共约定：
  /// 以Create开头一般返回创建后的对象ID。
  /// 以Get开头一般传入用户token和对应的对象ID，返回对象。
  /// </summary>
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
