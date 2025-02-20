namespace LLMRolePlay
{
  public class ApiResponse
  {
    public int StatusCode { get; private set; }
    public object? Data;
    public string Message;

    public ApiResponse(int statusCode, object? data = null, string message = "")
    {
      StatusCode = statusCode;
      Data = data;
      Message = message;
    }
  }
}
