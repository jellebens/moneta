using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moneta.Core;
using Moneta.Frontend.API.Bus;
using Moneta.Frontend.API.Hubs;
using Moneta.Frontend.API.Models.Instruments;
using Moneta.Frontend.API.Models.Yfapi;
using Moneta.Frontend.API.Services;
using Moneta.Frontend.Commands;
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
        private readonly IInstrumentService _InstrumentService;
        private readonly IHubContext<CommandHub> _Hub;
        private readonly IBus _Bus;

        public InstrumentsController(IYahooFinanceClient yahooFinanceClient,IInstrumentService instrumentService ,IHubContext<CommandHub> hub, IBus bus)
        {
            _YahooFinanceClient = yahooFinanceClient;
            _InstrumentService = instrumentService;
            _Hub = hub;
            _Bus = bus;
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

        [HttpPost]
        public async Task<IActionResult> Index(CreateInstrumentCommand createInstrument) {
            var token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);


            await _Bus.SendAsync(Queues.Frontend.Commands, token, createInstrument);

            CommandStatus status = CommandStatus.Queue(createInstrument.Id);

            await _Hub.Clients.All.SendAsync(createInstrument.Id.ToString(), status);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            _InstrumentService.Authenticate(token);
            try
            {
                InstrumentListItem[] instrumentListItems = await _InstrumentService.GetAsync();
                return Ok(instrumentListItems);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                throw;
            }
            
            
            
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
