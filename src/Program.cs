using LLMRolePlay;
using LLMRolePlay.Config;
using LLMRolePlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;


Config? config = Config.Load("config.json");
if (config == null)
{
  config = Config.Default();
  var f = System.IO.File.CreateText("config.json");
  f.Write(JsonSerializer.Serialize(config));
  f.Flush();
  f.Close();
}

string connectionString = config.dbConnectionString;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
if (config.dbType == "sqlite")
{
  builder.Services.AddDbContext<DBContext>(options => options.UseSqlite(connectionString));
}
else
{
  throw new Exception("Unsupported database type");
}
builder.Services.AddAuthentication(option =>
{
  option.AddScheme<TokenAuthentication>("TokenAuthentication", null);
  option.DefaultAuthenticateScheme = "TokenAuthentication";
  option.DefaultChallengeScheme = "TokenAuthentication";
  option.DefaultForbidScheme = "TokenAuthentication";
});

var app = builder.Build();

app.MapControllers();


using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
  using (var context = serviceScope.ServiceProvider.GetRequiredService<DBContext>())
  {
    if (await context.Database.EnsureCreatedAsync())
    {
      string password = Utils.GenerateRandomString(8);
      await User.CreateAdmin(context, "admin", "admin@example.com", Utils.Sha256Encode(password));
      Console.WriteLine($"Admin user created, UserName: admin, Email: admin@example.com, Password: {password}");
    }
  }
}

app.UseFileServer(new FileServerOptions
{
  FileProvider = new ManifestEmbeddedFileProvider(
  typeof(Program).Assembly, "webui/dist"
)
});

app.UseExceptionHandler(h =>
{
  h.Run(async context =>
  {
    context.Response.StatusCode = 200;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync("{\"code\":500,\"message\":\"Internal Server Error\"}");
  });
});

app.Run();
