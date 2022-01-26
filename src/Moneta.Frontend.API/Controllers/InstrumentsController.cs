using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneta.Frontend.API.Models;
using Moneta.Frontend.API.Models.rapidapi;
using Moneta.Frontend.API.Services;
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
            AutoCompleteResponse result = await _YahooFinanceClient.Search(q);

            List<InstrumentSearchResult> results = new List<InstrumentSearchResult>();

            foreach (Quote quote in result.Quotes)
            {
                InstrumentSearchResult r = new InstrumentSearchResult();
                r.Exchange = quote.Exchange;
                r.Symbol = quote.Symbol;
                r.Type = quote.QuoteType;
                r.Name = quote.Shortname;

                results.Add(r);
            }

            return Ok(results.ToArray());
        }
    }
}
