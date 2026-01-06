using Microsoft.AspNetCore.Identity;
using Saas.Web.Models;

namespace Saas.Web.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        const string adminEmail = "admin@admin.com";
        const string adminPassword = "Admin123!";
        const string adminRoleName = "Admin";

        // Criar role Admin se não existir
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(
                new ApplicationRole
                {
                    Name = adminRoleName,
                    NormalizedName = adminRoleName.ToUpperInvariant(),
                }
            );
        }

        // Criar usuário admin se não existir
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
        }
    }
}
