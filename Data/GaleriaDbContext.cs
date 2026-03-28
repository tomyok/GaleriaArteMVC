using GaleriaArte.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaleriaArte.Data
{
    public class GaleriaDbContext : IdentityDbContext
    {
        public GaleriaDbContext(DbContextOptions options): base(options) { }
        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<Exposicion> Exposiciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Obra>()
                .HasMany(x => x.ExposicionesObra)
                .WithMany(x => x.ObrasExpuestas)
                .UsingEntity<Dictionary<string, object>>(
                    "ExposicionObra",
                    l => l.HasOne<Exposicion>()
                        .WithMany()
                        .HasForeignKey("ExposicionId")
                        .OnDelete(DeleteBehavior.Restrict),
                    r => r.HasOne<Obra>()
                        .WithMany()
                        .HasForeignKey("ObraId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j =>
                    {
                        j.HasKey("ObraId", "ExposicionId");
                        j.ToTable("ExposicionObra");
                    });

            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                        .SelectMany(t => t.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Exposicion>().HasData(
                new Exposicion
                {
                    Id = 1,
                    Nombre = "Expo Especial",
                    FechaInicio = Convert.ToDateTime("10/10/2025"),
                    FechaFin = Convert.ToDateTime("10/11/2025")
                }
            );

        }
    }
}
