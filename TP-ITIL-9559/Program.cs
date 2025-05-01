using Microsoft.EntityFrameworkCore;
using TP_ITIL_9559.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

var connectionString = builder.Configuration.GetConnectionString("MyPostgresConnection") ?? throw new InvalidOperationException("Connection string 'MyPostgresConnection' not found.");
builder.Services.AddDbContext<ITILDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();