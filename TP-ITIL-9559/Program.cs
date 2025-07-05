using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TP_ITIL_9559.Data;
using TP_ITIL_9559.Interceptos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
var connectionString = Environment.GetEnvironmentVariable("MYPOSTGRESCONNECTION") ?? builder.Configuration.GetConnectionString("MyPostgresConnection") ?? throw new InvalidOperationException("Connection string 'MyPostgresConnection' not found.");
builder.Services.AddDbContext<ITILDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirConCredenciales", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();

        policy.WithOrigins("https://tp-itil-app.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseMiddleware<UserIdMiddleware>();
app.UseMiddleware<ExceptionHandler>();
app.UseAuthentication();

app.UseCors("PermitirConCredenciales");

app.UseAuthorization();

app.MapControllers();

app.Run();
