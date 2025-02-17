using LLMRolePlay.Apis;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var dbContext = new LLMRolePlay.Models.DbContext();

if (dbContext.Database.EnsureCreated())
{
  var password = Utils.GetRandomString(8);
  var username = "admin";
  var email = "user@example.com";
  var user = new LLMRolePlay.Models.User
  {
    UserName = username,
    Email = email,
    Password = password,
    Token = null
  };
  dbContext.Users.Add(user);
  dbContext.SaveChanges();
  Console.WriteLine($"Default user created: {username} / {email} / {password}");
}

var api = new Api(dbContext);
var app = builder.Build();

app.UseExceptionHandler(h =>
{
  h.Run(async context =>
  {
    context.Response.StatusCode = 200;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync("{\"code\":500,\"message\":\"Internal Server Error\"}");
  });
});

if (!env.IsDevelopment())
{
  app.UseFileServer(new FileServerOptions
  {
    FileProvider = new ManifestEmbeddedFileProvider(
    typeof(Program).Assembly, "webui/dist"
  )
  });
}

app.MapGroup("/api").MapPost("login", api.Login);

app.Run();
