﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransactionService.Sql;

namespace TransactionService.Migrations
{
    [DbContext(typeof(TransactionsDbContext))]
    [Migration("20220222133013_AddCashTransfer")]
    partial class AddCashTransfer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TransactionService.Domain.CashTransfer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id")
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("AccountId");

                    b.Property<decimal>("Amount")
                        .HasPrecision(38, 4)
                        .HasColumnType("decimal(38,4)")
                        .HasColumnName("Amount");

                    b.Property<long?>("CurrencyId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("Date");

                    b.HasKey("Id")
                        .IsClustered();

                    b.HasIndex("CurrencyId");

                    b.ToTable("CashTransfer", "transactions");
                });

            modelBuilder.Entity("TransactionService.Domain.Currency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("Symbol");

                    b.HasKey("Id")
                        .IsClustered();

                    b.ToTable("Currency", "instruments");
                });

            modelBuilder.Entity("TransactionService.Domain.CashTransfer", b =>
                {
                    b.HasOne("TransactionService.Domain.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.Navigation("Currency");
                });
#pragma warning restore 612, 618
        }
    }
}
