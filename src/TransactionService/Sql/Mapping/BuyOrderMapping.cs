using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TransactionService.Domain;

namespace TransactionService.Sql.Mapping
{
    public class BuyOrderMapping : IEntityTypeConfiguration<BuyOrder>
    {
        public void Configure(EntityTypeBuilder<BuyOrder> builder)
        {
            builder.ToTable("Transactions", "BuyOrder");

            builder.HasKey(t => t.Id);

            builder.HasAlternateKey(x => new { x.AccountId, x.Number }).IsClustered();

            builder.Property(t => t.AccountId)
                   .HasColumnName("AccountId");

            builder.Property(t => t.Currency).HasColumnName("Currency").HasMaxLength(3);

            builder.Property(t => t.Date).HasColumnName("Date");

            builder.Property(t => t.Id).HasColumnName("Id");
            
            builder.Property(t => t.Number).HasColumnName("Number");

            builder.Property(t => t.Symbol).HasColumnName("Symbol");

            builder.Property(t => t.UserId).HasColumnName("UserId");

            



        }
    }
}
