using InstrumentService.Contracts.Data;
using InstrumentService.Domain;
using InstrumentService.Sql;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneta.Core.Jwt;
using System.Security.Claims;

namespace InstrumentService.Controllers
{
    [Route("instruments")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly ILogger<InstrumentsController> _Logger;
        private readonly InstrumentsDbContext _InstrumentsDbContext;

        public InstrumentsController(ILogger<InstrumentsController> logger, InstrumentsDbContext instrumentsDbContext)
        {
            _Logger = logger;
            _InstrumentsDbContext = instrumentsDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {

            InstrumentInfo[] results =  _InstrumentsDbContext.Instruments.Where(i => !i.IsDeleted).Select(i => new InstrumentInfo { 
                Id = i.Id,
                Name = i.Name,
                Currency =  i.Currency.Symbol,
                Symbol = i.Symbol
            }).ToArray();

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateInstrumentCommand createInstrument) {

            bool exists = _InstrumentsDbContext.Instruments.Any(i => i.Symbol == createInstrument.Symbol);

            if (exists)
            {
                _Logger.LogCritical($"Error creating instrument with ticker symbol {createInstrument.Symbol}. Instrument with Ticker symbol {createInstrument.Symbol} allready exits");
                return BadRequest($"Instrument with Ticker symbol {createInstrument.Symbol} allready exits");
            }

            Currency currency = _InstrumentsDbContext.Currencies.SingleOrDefault(c => c.Symbol.ToUpper() == createInstrument.Currency.ToUpper());

            if(currency == null)
            {
                currency = new Currency
                {
                    Symbol = createInstrument.Currency.ToUpper()
                };

                await _InstrumentsDbContext.Currencies.AddAsync(currency);
            }

            Sector sector = _InstrumentsDbContext.Sectors.SingleOrDefault(s => s.Id == createInstrument.Sector);

            if (sector == null) {
                _Logger.LogCritical($"Error creating instrument with ticker symbol {createInstrument.Symbol}. Sector with id {0} does not exist");
                return BadRequest($"Sector with id {0} does not exist");
            }

            Instrument instrument = new Instrument
            {
                Symbol = createInstrument.Symbol,
                Name = createInstrument.Name,
                Currency = currency,
                IsDeleted = false,
                Isin = createInstrument.Isin,
                Exchange = createInstrument.Exchange,
                Sector = sector
            };

            await _InstrumentsDbContext.AddAsync(instrument);
            await _InstrumentsDbContext.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {
            //TODO implement roles
            Instrument instrument = _InstrumentsDbContext.Instruments.SingleOrDefault(i => i.Id == id);

            if (instrument == null) {
                _Logger.LogCritical($"Error deleting instrument with id {id}. Instrument with Id {id} does not exist");
                StatusCode(StatusCodes.Status404NotFound ,$"Instrrument with Id {id} does not exist");
            }

            _InstrumentsDbContext.Remove(instrument);

            _InstrumentsDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
