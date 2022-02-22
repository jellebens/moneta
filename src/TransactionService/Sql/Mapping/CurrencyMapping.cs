using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain;

namespace TransactionService.Sql.Mapping
{
    public class CurrencyMapping : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currency", "instruments");
            builder.HasKey(c => c.Id)
                   .IsClustered();

            
            builder.Property(c => c.Id)
                   .HasColumnName("Id")
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(c => c.Symbol)
                   .HasColumnName("Symbol")
                   .HasMaxLength(3)
                   .IsRequired();

        }
    }
}
