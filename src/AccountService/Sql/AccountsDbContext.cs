using AccountService.Domain;
using AccountService.Sql.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Sql
{
    public class AccountsDbContext : DbContext
    {
        public AccountsDbContext([NotNull] DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountMapping());
        }

        public DbSet<Account> Accounts { get; set; }

    }
}
