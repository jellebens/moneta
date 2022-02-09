using InstrumentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstrumentService.Sql.Mapping
{
    public class SectorMapping : IEntityTypeConfiguration<Sector>
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            builder.ToTable("Sector", "instruments");
            builder.HasKey(c => c.Id)
                   .IsClustered();


            builder.Property(c => c.Id)
                   .HasColumnName("Id")                   
                   .IsRequired();

            builder.Property(c => c.Name)
                   .HasColumnName("Name")
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}
