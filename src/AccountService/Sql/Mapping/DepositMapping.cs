using AccountService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Sql.Mapping
{
    public class DepositMapping : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.ToTable("Deposit", "accounts");
            builder.HasKey(d => d.Id)
                   .IsClustered();

            builder.Property(d => d.Id)
                   .HasColumnName("Id")
                   .IsRequired();

            builder.Property(d => d.Amount)
                   .HasColumnName("Amount")
                   .HasPrecision(34, 4);

            builder.Property(d => d.Year)
                   .HasColumnName("Year")
                   .IsRequired();

            builder.HasOne(a => a.Account)
                   .WithMany();
        }
    }
}
