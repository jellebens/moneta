using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstrumentService.Sql
{
    public class DatabaseUpgradeFilter : IStartupFilter
    {
        private readonly ILogger<DatabaseUpgradeFilter> _Logger;

        public DatabaseUpgradeFilter(ILogger<DatabaseUpgradeFilter> logger)
        {
            _Logger = logger;
        }
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                _Logger.LogInformation("Starting Upgrade of database");

                using (var scope = builder.ApplicationServices.CreateScope()) {
                    InstrumentsDbContext context = scope.ServiceProvider.GetRequiredService<InstrumentsDbContext>();
                    
                    context.Database.Migrate();
                }
                
                _Logger.LogInformation("Finished Upgrade of database");
                next(builder);
            };
        }
    }
}
