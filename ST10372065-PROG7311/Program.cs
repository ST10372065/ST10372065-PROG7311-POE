using Microsoft.EntityFrameworkCore;
using ST10372065_PROG7311.Data;
using ST10372065_PROG7311.Services;

namespace ST10372065_PROG7311
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=local.db"));

            // Add cookie-based authentication
            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Home/HomePage"; // Path to the home page
                    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Cookie expiration time
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=HomePage}/{id?}");

            app.Run();
        }
    }
}
