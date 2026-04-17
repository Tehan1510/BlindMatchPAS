using BlindMatchPAS.Data;
using BlindMatchPAS.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC only - we don't use Razor Pages
builder.Services.AddControllersWithViews();

// Database - SQL Server connectivity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// RBAC - Role Based Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentOnly", policy =>
        policy.RequireRole("Student"));

    options.AddPolicy("SupervisorOnly", policy =>
        policy.RequireRole("Supervisor"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IMatchService, MatchService>();

var app = builder.Build();

// ============================================================
// AUTO MIGRATE AND SEED DATABASE
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Step 1 - Apply migrations
    db.Database.Migrate();
    // Step 2 - Seed default accounts and sample data
    await DbSeeder.SeedAsync(db);
}
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Must be in this exact order
app.UseAuthentication();
app.UseAuthorization();

// Ignore the /Pages/ folder routes - MVC only
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Splash}/{id?}");

app.Run();