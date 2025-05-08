using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using FoodCentral.Controllers; // your DbContext is in Controllers
using FoodCentral.Models;

namespace FoodCentral
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<FoodCentralDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity without default UI
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<FoodCentralDbContext>()
                .AddDefaultTokenProviders();

            // Global authorization policy (requires login)
            builder.Services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // Add Razor Pages (needed for Identity to work properly)
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Seed roles and dummy users
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedRolesAndUsersAsync(services).Wait();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); // still required

            app.Run();
        }

        private static async Task SeedRolesAndUsersAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var admin = new IdentityUser { UserName = "admin@foodcentral.com", Email = "admin@foodcentral.com", EmailConfirmed = true };
            var user1 = new IdentityUser { UserName = "user1@foodcentral.com", Email = "user1@foodcentral.com", EmailConfirmed = true };
            var user2 = new IdentityUser { UserName = "user2@foodcentral.com", Email = "user2@foodcentral.com", EmailConfirmed = true };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            if (await userManager.FindByEmailAsync(user1.Email) == null)
            {
                await userManager.CreateAsync(user1, "User123!");
                await userManager.AddToRoleAsync(user1, "User");
            }
            if (await userManager.FindByEmailAsync(user2.Email) == null)
            {
                await userManager.CreateAsync(user2, "User123!");
                await userManager.AddToRoleAsync(user2, "User");
            }
        }
    }
}
