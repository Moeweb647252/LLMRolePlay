namespace LLMRolePlay
{
  public class Response
  {
    public int StatusCode { get; set; }
    public object? Data { get; set; }
    public string StatusMessage { get; set; }

    public Response(int statusCode, object? data = null, string statusMessage = "")
    {
      StatusCode = statusCode;
      Data = data;
      StatusMessage = statusMessage;
    }
  }
}