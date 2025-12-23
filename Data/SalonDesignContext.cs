using System.Data.Entity;
using SalonDesign.Models;

namespace SalonDesign.Data
{
    /// <summary>
    /// Entity Framework DbContext for Salon Design database
    /// </summary>
    public class SalonDesignContext : DbContext
    {
        public SalonDesignContext() : base("name=SalonDesignConnection")
        {
        }

        public DbSet<Salon> Salons { get; set; }
        public DbSet<SalonObject> SalonObjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Objects)
                .WithRequired(o => o.Salon)
                .HasForeignKey(o => o.SalonId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SalonObject>()
                .Property(o => o.ObjectType)
                .IsRequired();

            modelBuilder.Entity<SalonObject>()
                .Property(o => o.ShapeType)
                .IsRequired();
        }
    }
}
