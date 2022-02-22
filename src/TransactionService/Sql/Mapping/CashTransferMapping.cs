using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain;

namespace TransactionService.Sql.Mapping
{
    public class CashTransferMapping : IEntityTypeConfiguration<CashTransfer>
    {
        public void Configure(EntityTypeBuilder<CashTransfer> builder)
        {
            builder.ToTable("CashTransfer", "transactions");
            builder.HasKey(i => i.Id)
                   .IsClustered();

            builder.Property(t => t.Id)
                   .HasColumnName("Id")
                   .HasDefaultValueSql("newsequentialid()")
                   .IsRequired();

            builder.HasOne(t => t.Currency)
                .WithMany();

            builder.Property(t => t.Amount)
                .HasColumnName("Amount")
                .HasPrecision(38, 4)
                .IsRequired();
            
            builder.Property(t => t.Date)
                .HasColumnName("Date")
                .IsRequired();

            builder.Property(t => t.AccountId)
                .HasColumnName("AccountId")
                .IsRequired();
        }
    }
}
