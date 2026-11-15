
using Microsoft.EntityFrameworkCore;
using AutenticacionRoles.Models;

public class AutenticacionRolesContext(DbContextOptions<AutenticacionRolesContext> options) : DbContext(options)
{
    // Tabla de películas.
    public DbSet<Pelicula> Pelicula { get; set; } = default!;

    // Tabla de géneros.
    public DbSet<Genero> Genero { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Un género puede tener varias películas.
        // Cada película pertenece a un género.
        modelBuilder.Entity<Pelicula>()
            .HasOne(p => p.Genero)
            .WithMany(g => g.Peliculas)
            .HasForeignKey(p => p.GeneroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
