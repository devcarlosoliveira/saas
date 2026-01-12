using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saas.Web.Data;
using Saas.Web.Data.Interceptors;
using Saas.Web.Data.Seed;
using Saas.Web.Data.Seeds;
using Saas.Web.Models;
using Saas.Web.Services.Document;
using Saas.Web.Services.IA;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(
        connectionString,
        sqlite => sqlite.ExecutionStrategy(deps => new SqliteRetryingExecutionStrategy(deps))
    );
    options.AddInterceptors(new SqliteOptimizationInterceptor(), new AuditInterceptor());
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        options.SignIn.RequireConfirmedAccount = true
    )
    .AddDefaultUI() // Adiciona as páginas padrão do Identity
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();

// Registrar serviços de IA e processamento
builder.Services.AddScoped<ISemanticKernelService, SemanticKernelService>();
builder.Services.AddScoped<IContentProcessingService, ContentProcessingService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await db.Database.MigrateAsync();

    await IdentitySeeder.SeedAsync(scope.ServiceProvider);

    // Seed de Tags
    await TagSeeder.SeedTagsAsync(db);

    await db.Database.ExecuteSqlRawAsync("PRAGMA optimize;");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Rota para áreas (necessário para /app funcionar)
app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    )
    .WithStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages().WithStaticAssets();

app.Run();
