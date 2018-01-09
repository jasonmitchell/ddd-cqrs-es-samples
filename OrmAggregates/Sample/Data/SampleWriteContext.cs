using Microsoft.EntityFrameworkCore;
using Sample.Data.Mapping;
using Sample.Data.Model;

namespace Sample.Data
{
    public class SampleWriteContext : DbContext
    {
        public DbSet<ReservationMemento> ReservationMementos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(@"Host=localhost;Port=4001;Database=orm_sample;Username=docker;Password=docker");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReservationMementoMapping());
        }
    }
}
