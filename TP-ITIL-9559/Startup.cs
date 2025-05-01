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
        // Use exception handling middleware to handle any exceptions thrown by the app
        app.UseExceptionHandler("/error");

        // Use HSTS middleware to enforce HTTPS usage
        app.UseHsts();

        // Use HTTPS redirection middleware to redirect all non-HTTPS requests to HTTPS
        app.UseHttpsRedirection();

        // Use static file middleware to serve static files (e.g., HTML, CSS, JS) from the 'wwwroot' folder
        app.UseStaticFiles();

        // Use routing middleware to map incoming requests to the appropriate action method
        app.UseRouting();

        // Use authentication middleware to handle user authentication and authorization
        app.UseAuthentication();

        // Use authorization middleware to apply authorization policies to the incoming requests
        app.UseAuthorization();

        // Use endpoints middleware to map incoming requests to the appropriate controller action method
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Migrate the database schema on startup (if necessary)
        dbContext.Database.Migrate();
    }

}
