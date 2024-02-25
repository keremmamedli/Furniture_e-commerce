using AmadoApp.Business.Extensions;
using AmadoApp.Core.Entities.Account;
using AmadoApp.DAL.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AmadoApp.MVC
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Servisleri konteynere ekle.
            builder.Services.AddRepositories();
            builder.Services.AddServices(); // AddServices() uzant? yöntemini ça??r?n.

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            // ASP.NET Core Kimlik do?rulamas?n? yap?land?r?n.
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Parola ayarlar?.
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                // Kilitlenme ayarlar?.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Kullan?c? ayarlar?.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Uygulama çerez ayarlar?n? yap?land?r?n.
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDeniedCustom";
            });

            builder.Services.AddAuthentication().AddCookie();

            var app = builder.Build();

            // HTTP istek hatt?n? yap?land?r?n.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
