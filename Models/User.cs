namespace LLMRolePlay.Models
{
  public class User
  {
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Token { get; set; }
  }
}