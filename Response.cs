namespace LLMRolePlay
{
  public enum StatusCode
  {
    Success = 200,
    TokenError = 501
  }
  public class ApiResponse
  {
    public int code { get; set; }
    public object? data { get; set; }
    public string msg { get; set; }

    public ApiResponse(int statusCode, object? data = null, string statusMessage = "")
    {
      code = statusCode;
      this.data = data;
      msg = statusMessage;
    }

    public static ApiResponse Success(object? data = null, string statusMessage = "")
    {
      return new ApiResponse(200, data, statusMessage);
    }

    public static ApiResponse Message(int statusCode, string Message)
    {
      return new ApiResponse(statusCode, null, Message);
    }

    public static ApiResponse TokenError()
    {
      return new ApiResponse(501, null, "token error");
    }
  }
}