namespace LLMRolePlay
{
  public class Response
  {
    public int code { get; set; }
    public object? data { get; set; }
    public string msg { get; set; }

    public Response(int statusCode, object? data = null, string statusMessage = "")
    {
      code = statusCode;
      this.data = data;
      msg = statusMessage;
    }
  }
}