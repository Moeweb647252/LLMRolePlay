namespace LLMRolePlay
{
  public enum StatusCode
  {
    Success = 200,
    TokenError = 501
  }
  public class ApiResponse
  {
    public int StatusCode { get; set; }
    public object? Data { get; set; }
    public string Message { get; set; }

    public ApiResponse(int statusCode, object? data = null, string message = "")
    {
      StatusCode = statusCode;
      Data = data;
      Message = message;
    }

    public static ApiResponse Success(object? data = null, string message = "") => new ApiResponse(200, data, message);

    public static ApiResponse MessageOnly(int statusCode, string message) => new ApiResponse(statusCode, null, message);
    public static ApiResponse TokenError() => new ApiResponse(501, null, "token error");
  }
}