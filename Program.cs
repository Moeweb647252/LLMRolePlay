using LLMRolePlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;


var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LLMRolePlay");
Directory.CreateDirectory(directory);
string connectionString = $"Data Source={Path.Combine(directory, "LLMRolePlay.db")}";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<DBContext>(o => o.UseSqlite(connectionString));

var app = builder.Build();

app.MapControllers();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
  var context = serviceScope.ServiceProvider.GetRequiredService<DBContext>();
  if (context.Database.EnsureCreated())
  {
    var password = Utils.GenerateRandomString(8);
    await User.CreateAdmin(context, "admin", "admin@example.com", password);
    Console.WriteLine($"Init account created admin / admin@example.com / {password}");
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
