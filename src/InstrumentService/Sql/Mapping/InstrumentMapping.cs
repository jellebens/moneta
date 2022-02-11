using InstrumentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstrumentService.Sql.Mapping
{
    public class InstrumentMapping : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.ToTable("Instrument", "instruments");
            builder.HasKey(i => i.Id)
                   .IsClustered();


            builder.Property(i => i.Id)
                   .HasColumnName("Id")
                   .HasDefaultValueSql("newsequentialid()")
                   .IsRequired();

            builder.HasOne(i => i.Currency)
                .WithMany();
            
            builder.HasOne(i => i.Sector)
                .WithMany();

            builder.Property(i => i.Isin)
                .HasColumnName("Isin")
                .HasColumnType("nchar(35)")
                .IsRequired()
                .HasMaxLength(35);

            builder.Property(i => i.IsDeleted)
                .HasColumnName("IsDeleted")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(i => i.Name)
                .HasColumnName("Name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(i => i.Symbol)
                .HasColumnName("Symbol")
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
