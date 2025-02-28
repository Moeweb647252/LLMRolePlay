using System.Security.Cryptography;
using System.Text;

public class Utils
{
  public static string GenerateRandomString(int length)
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, length)
      .Select(s => s[random.Next(s.Length)]).ToArray());
  }

  public static string Sha256Encode(string input)
  {
    using (SHA256 sha256Hash = SHA256.Create())
    {
      byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
      StringBuilder builder = new StringBuilder();
      for (int i = 0; i < bytes.Length; i++)
      {
        builder.Append(bytes[i].ToString("x2"));
      }
      return builder.ToString();
    }
  }

  public async static Task<List<T>> AwaitAll<T>(IEnumerable<Task<T>> tasks)
  {
    return (await Task.WhenAll(tasks)).ToList();
  }

}