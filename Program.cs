using LLMRolePlay.APIs;
using LLMRolePlay.Models;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSqlite<DBContext>(DBContext.DbPath);

var app = builder.Build();

app.MapControllers();

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
