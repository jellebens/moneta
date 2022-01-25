using InstrumentService.Domain;
using InstrumentService.Sql.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace InstrumentService.Sql
{
    public class InstrumentsDbContext: DbContext
    {
        public InstrumentsDbContext([NotNull] DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new InstrumentMapping());
            modelBuilder.ApplyConfiguration(new CurrencyMapping());
        }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Instrument> Instruments { get; set; }
    }
}
