namespace LLMRolePlay.Apis
{
  public class ApiResponse
  {
    public int code { get; set; }
    public string? message { get; set; }
    public object? data { get; set; }
  }
}