using LLMRolePlay;
using LLMRolePlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;


var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LLMRolePlay");
Directory.CreateDirectory(directory);
string dbPath = Path.Combine(directory, "LLMRolePlay.db");
string connectionString = $"Data Source={dbPath}";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<DBContext>(options => options.UseSqlite(connectionString));
builder.Services.AddAuthentication(option =>
{
  option.AddScheme<TokenAuthentication>("TokenAuthentication", null);
  option.DefaultAuthenticateScheme = "TokenAuthentication";
  option.DefaultChallengeScheme = "TokenAuthentication";
  option.DefaultForbidScheme = "TokenAuthentication";
});

var app = builder.Build();

app.MapControllers();


using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
  using (var context = serviceScope.ServiceProvider.GetRequiredService<DBContext>())
  {
    if (await context.Database.EnsureCreatedAsync())
    {
      string password = Utils.GenerateRandomString(8);
      await User.CreateAdmin(context, "admin", "admin@example.com", password);
      Console.WriteLine($"Admin user created, UserName: admin, Email: admin@example.com, Password: {password}");
    }
  }
}

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}
else
{
  app.UseFileServer(new FileServerOptions
  {
    FileProvider = new ManifestEmbeddedFileProvider(
    typeof(Program).Assembly, "webui/dist"
  )
  });
}

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
