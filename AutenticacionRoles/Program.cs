
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutenticacionRoles.Models;

var builder = WebApplication.CreateBuilder(args);

// Cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("AutenticacionRolesContext")
    ?? throw new InvalidOperationException(
        "Connection string 'AutenticacionRolesContext' not found.");

// Contexto de Identity
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

// Contexto de películas y géneros
builder.Services.AddDbContext<AutenticacionRolesContext>(options =>
    options.UseSqlServer(connectionString));

// Configuración de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

// Configuración de cookies de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Servicios MVC y Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Crear roles y usuarios de prueba al iniciar la aplicación (Fase 5)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1. Crear los roles del Grupo 7 si no existen
    string[] roles = { "Curador", "Espectador" };
    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    // 2. Crear usuario de gestión (Curador) y asignar su rol
    string emailCurador = "curador@cine.com";
    var userCurador = await userManager.FindByEmailAsync(emailCurador);
    if (userCurador == null)
    {
        userCurador = new ApplicationUser
        {
            UserName = emailCurador,
            Email = emailCurador,
            EmailConfirmed = true
        };
        var resCurador = await userManager.CreateAsync(userCurador, "Curador2026*");
        if (!resCurador.Succeeded)
        {
            throw new Exception("No se pudo crear Curador: " + string.Join(", ", resCurador.Errors.Select(e => e.Description)));
        }
    }
    if (!await userManager.IsInRoleAsync(userCurador, "Curador"))
    {
        await userManager.AddToRoleAsync(userCurador, "Curador");
    }

    // 3. Crear usuario de consulta (Espectador) y asignar su rol
    string emailEspectador = "espectador@cine.com";
    var userEspectador = await userManager.FindByEmailAsync(emailEspectador);
    if (userEspectador == null)
    {
        userEspectador = new ApplicationUser
        {
            UserName = emailEspectador,
            Email = emailEspectador,
            EmailConfirmed = true
        };
        var resEspectador = await userManager.CreateAsync(userEspectador, "Espectador2026*");
        if (!resEspectador.Succeeded)
        {
            throw new Exception("No se pudo crear Espectador: " + string.Join(", ", resEspectador.Errors.Select(e => e.Description)));
        }
    }
    if (!await userManager.IsInRoleAsync(userEspectador, "Espectador"))
    {
        await userManager.AddToRoleAsync(userEspectador, "Espectador");
    }
}

// Configuración del entorno
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Archivos estáticos y páginas Razor
app.MapStaticAssets();
app.MapRazorPages();

// Ruta principal
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
