using Microsoft.EntityFrameworkCore;

public class AutenticacionRolesContext(DbContextOptions<AutenticacionRolesContext> options) : DbContext(options)
{
    public DbSet<AutenticacionRoles.Models.Pelicula> Pelicula { get; set; } = default!;
}
