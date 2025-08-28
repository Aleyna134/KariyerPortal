using KariyerPortal.Models;
using Microsoft.AspNetCore.Identity;


namespace KariyerPortal.Seed;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
         var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
         var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

         // Roller
         var roles = new[] { "Admin", "User" };

         foreach (var roleName in roles)
         {
             if (!await roleManager.RoleExistsAsync(roleName))
             {
                 await roleManager.CreateAsync(new AppRole
                 {
                      Name=roleName
                 });
             }
         }

         // Admin Kullanıcısı
         var adminEmail = "admin@example.com";
         var adminUser = await userManager.FindByEmailAsync(adminEmail);
         if (adminUser == null)
         {
             adminUser = new AppUser
         {
                 UserName = "admin",
                 Email = adminEmail,
                  AdSoyad = "Admin"
             };
             await userManager.CreateAsync(adminUser, "Admin123!");
             await userManager.AddToRoleAsync(adminUser, "Admin");
         }

         // Normal Kullanıcı
         var normalEmail = "user@example.com";
         var normalUser = await userManager.FindByEmailAsync(normalEmail);
         if (normalUser == null)
         {
             normalUser = new AppUser
             {
                 UserName = "user",
                 Email = normalEmail,
                  AdSoyad = "Normal Kullanıcı"
             };
             await userManager.CreateAsync(normalUser, "User123!");
             await userManager.AddToRoleAsync(normalUser, "User");
         }
    }
}