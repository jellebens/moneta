﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Domain;
using TransactionService.Sql.Mapping;

namespace TransactionService.Sql
{
    public class TransactionsDbContext : DbContext
    {
        public TransactionsDbContext([NotNull] DbContextOptions options) : base(options)
        {
            
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BuyOrderMapping());
            modelBuilder.ApplyConfiguration(new CostsMapping());
        }

        public DbSet<BuyOrder> BuyOrders { get; set; }


        public DbSet<Cost> Costs { get; set; }

    }
}
