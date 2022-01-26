using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneta.Frontend.API.Models;
using Moneta.Frontend.API.Models.Yfapi;
using Moneta.Frontend.API.Services;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/instruments")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly IYahooFinanceClient _YahooFinanceClient;

        public InstrumentsController(IYahooFinanceClient yahooFinanceClient)
        {
            _YahooFinanceClient = yahooFinanceClient;
        }

        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(InstrumentSearchResult[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string q) {
            AutoCompleteResponse response = await _YahooFinanceClient.Search(q);

            List<InstrumentSearchResult> results = new List<InstrumentSearchResult>();

            foreach (Result result in response.ResultSet.Result)
            {
                InstrumentSearchResult r = new InstrumentSearchResult();
                r.Exchange = result.Exch;
                r.Symbol = result.Symbol;
                r.Type = MapType(result.Type);
                r.Name = result.Name;

                results.Add(r);
            }

            return Ok(results.ToArray());
        }

        private string MapType(string type)
        {
            switch (type.ToUpper()) {
                case "E":
                    return "EFT";
                case "S":
                    return "Stock";
                case "I":
                    return "Index";
                case "M":
                    return "Mutual Fund";
                default:
                    return type.ToUpper();
            }
        }
    }
}
