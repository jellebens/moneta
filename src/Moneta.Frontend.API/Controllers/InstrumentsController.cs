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

        [HttpGet("search/{symbol}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(InstrumentSearchResult[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string symbol) {
            AutoCompleteResponse response = await _YahooFinanceClient.Search(symbol);

            List<InstrumentSearchResult> results = new List<InstrumentSearchResult>();

            foreach (AutoCompleteResult result in response.ResultSet.Result)
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

        [HttpGet("{symbol}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(InstrumentSearchResult[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string symbol)
        {
            QuoteResults responses = await _YahooFinanceClient.Detail(symbol);

            if (responses.QuoteResponse.Result.Count != 1) {
                BadRequest($"Only one result was expected bu got {responses.QuoteResponse.Result.Count}");
            }

            QuoteResult result = responses.QuoteResponse.Result[0];

            InstrumentDetailResult retVal = new InstrumentDetailResult();
            retVal.Name = result.LongName;
            retVal.Symbol = result.Symbol;
            retVal.Type = MapQuoteType(result.QuoteType);
            retVal.Exchange = result.Exchange;
            retVal.Currency = result.Currency;

            return Ok(retVal);
        }

        private string MapQuoteType(string quoteType)
        {
            switch (quoteType.ToUpper())
            {
                case "EFT":
                    return "EFT";
                case "EQUITY":
                    return "Stock";
                case "INDEX":
                    return "Index";
                case "MUTUALFUND":
                    return "Mutual Fund";
                default:
                    return quoteType.ToUpper();
            }
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
