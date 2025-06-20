using Microsoft.EntityFrameworkCore;
using TP_ITIL_9559.Data;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ITILDbContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("MYPOSTGRESCONNECTION") ?? Configuration.GetConnectionString("MyPostgresConnection")));

        services.AddControllers();
        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITILDbContext dbContext)
    {
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        dbContext.Database.Migrate();
    }
}
