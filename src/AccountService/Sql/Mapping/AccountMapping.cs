using AccountService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Sql.Mapping
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account", "account");
            builder.HasKey(a => a.Id)
                   .IsClustered();
            
            builder.Property(a => a.Id)
                   .HasColumnName("Id")
                   .IsRequired();
            
            builder.Property(a => a.Currency)
                   .HasColumnName("Currency")
                   .HasMaxLength(3);

            builder.Property(a => a.Name)
                   .HasColumnName("Name")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(a => a.Owner)
                   .HasColumnName("Owner")
                   .HasMaxLength(255)
                   .IsRequired();

          }
    }
}
