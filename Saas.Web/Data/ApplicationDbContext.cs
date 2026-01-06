using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saas.Web.Models;

namespace Saas.Web.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>,
        IDataProtectionKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Renomear todas as tabelas do Identity
        builder.Entity<ApplicationUser>().ToTable("AppUsers");
        builder.Entity<ApplicationRole>().ToTable("AppRoles");
        builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens");
        // Renomear a tabela do DataProtectionKey
        builder.Entity<DataProtectionKey>().ToTable("AppDataProtectionKeys");
    }
}
