using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportTransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportService : ControllerBase
    {
        private readonly ILogger<ImportService> _Logger;

        public ImportService(ILogger<ImportService> logger)
        {
            _Logger = logger;
        }

        [HttpPost]
        [Route("Create")]
        public void FromLines(string import)
        {
            string[] lines = import.Split('\n');
            _Logger.LogInformation($"Importing {0} transactions", lines.Length);

        }
    }
}
