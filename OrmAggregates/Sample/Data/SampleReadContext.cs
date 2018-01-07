using Microsoft.EntityFrameworkCore;
using Sample.Data.Mapping;
using Sample.Data.Model;

namespace Sample.Data
{
    public class SampleReadContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use a read only user here
                optionsBuilder.UseNpgsql(@"Host=localhost;Port=4001;Database=orm_sample;Username=docker;Password=docker");
            }

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReservationMapping());
        }
    }
}
