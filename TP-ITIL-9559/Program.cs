using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TP_ITIL_9559.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

var connectionString = builder.Configuration.GetConnectionString("MyPostgresConnection") ?? throw new InvalidOperationException("Connection string 'MyPostgresConnection' not found.");
builder.Services.AddDbContext<ITILDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware<UserIdMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();