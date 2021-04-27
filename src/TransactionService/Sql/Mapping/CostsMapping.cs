using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Domain;

namespace TransactionService.Sql.Mapping
{
    public class CostsMapping : IEntityTypeConfiguration<Cost>
    {
        public void Configure(EntityTypeBuilder<Cost> builder)
        {
            builder.ToTable("costs", "transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("Id");

            builder.Property(t => t.Type)
                .HasColumnName("Type");

            builder.Property(t => t.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(19,5)")
                .HasPrecision(19, 5);
            
            
            builder.HasOne(t => t.Buyorder).WithMany(t => t.Costs);
        }
    }
}
