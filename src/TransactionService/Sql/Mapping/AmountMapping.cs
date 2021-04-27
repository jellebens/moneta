using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Domain;

namespace TransactionService.Sql.Mapping
{
    public class AmountMapping: IEntityTypeConfiguration<Amount>
    {
        
        public void Configure(EntityTypeBuilder<Amount> builder)
        {
            builder.ToTable("amount", "transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("Id");

            builder.Property(t => t.Currency)
                .HasColumnName("Currency")
                .HasColumnType("char(3)")
                .HasMaxLength(3);

            builder.Property(t => t.Quantity)
                .HasColumnName("Quantity")
                .HasColumnType("decimal(19,5)")
                .HasPrecision(19, 5);

            builder.Property(t => t.Price)
                .HasColumnName("Price")
                .HasColumnType("decimal(19,5)")
                .HasPrecision(19, 5);

            

        }
    }
}
