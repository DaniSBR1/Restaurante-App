using Microsoft.EntityFrameworkCore;
using RestauranteApp.Model;

namespace RestauranteApp.DA.Context
{
    public class RestauranteContext : DbContext
    {
        public RestauranteContext(DbContextOptions<RestauranteContext> options)
            : base(options)
        {
        }

        public DbSet<Mesa> Mesas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mesa>(entity =>
            {
                entity.ToTable("Mesas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NumeroMesa).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.Capacidad).IsRequired();
                entity.Property(e => e.Seccion).IsRequired().HasColumnType("varchar(max)");
                entity.Property(e => e.PrecioPorPersona).IsRequired().HasColumnType("money");
                entity.Property(e => e.IdentificacionCliente).HasColumnType("varchar(max)");
                entity.Property(e => e.NombreCliente).HasColumnType("varchar(max)");
                entity.Property(e => e.FechaReserva).HasColumnType("date");
                entity.Property(e => e.DepositoDeGarantia).HasColumnType("money");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
