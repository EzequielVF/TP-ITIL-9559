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
        // Register the DbContext with the connection string
        services.AddDbContext<ITILDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("MyPostgresConnection")));

        services.AddControllers();
        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITILDbContext dbContext)
    {
        app.UseExceptionHandler("/error");
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
