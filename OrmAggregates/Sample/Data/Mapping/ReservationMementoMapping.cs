using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Data.Model;

namespace Sample.Data.Mapping
{
    public class ReservationMementoMapping : IEntityTypeConfiguration<ReservationMemento>
    {
        public void Configure(EntityTypeBuilder<ReservationMemento> builder)
        {
            builder.ToTable("reservation_memento");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.TicketId)
                .HasColumnName("ticket_id");

            builder.Property(x => x.RequestedQuantity)
                .HasColumnName("requested_quantity");

            builder.Property(x => x.ReservedQuantity)
                .HasColumnName("reserved_quantity");

            builder.Property(x => x.Status)
                .HasColumnName("status");

            builder.Property(x => x.LastUpdated)
                .HasColumnName("last_updated");
        }
    }
}