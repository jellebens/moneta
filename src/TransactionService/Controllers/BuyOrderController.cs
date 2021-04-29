using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Core.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TransactionService.Contracts.Data;
using TransactionService.Domain;
using TransactionService.Events;
using TransactionService.Services;
using TransactionService.Sql;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Controllers
{
    [Route("buyorders")]
    [ApiController]
    public class BuyOrderController : ControllerBase
    {
        private readonly ILogger<BuyOrderController> _Logger;
        private readonly IConfiguration _Configuration;
        private readonly TransactionsDbContext _TransactionsDbContext;
        private readonly IAccountsService _AccountService;
        private readonly IJwtTokenBuilder _JwtTokenBuilder;

        public BuyOrderController(ILogger<BuyOrderController> logger, IConfiguration configuration, TransactionsDbContext transactionsDbContext, IAccountsService accountService, IJwtTokenBuilder jwtTokenBuilder)
        {
            _Logger = logger;
            _Configuration = configuration;
            _TransactionsDbContext = transactionsDbContext;
            _AccountService = accountService;
            _JwtTokenBuilder = jwtTokenBuilder;
        }

        [HttpPost("new")]
        public async Task<IActionResult> New([FromBody] NewBuyOrderCommand createBuyOrder, CancellationToken cancellationToken)
        {

            _Logger.LogInformation($"Creating Buy Order with Id: {createBuyOrder.Id} and number {createBuyOrder.TransactionNumber}");

            Claim userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (_TransactionsDbContext.BuyOrders.Any(x => x.AccountId == createBuyOrder.AccountId && x.Number == createBuyOrder.TransactionNumber))
            {
                _Logger.LogWarning($"Transaction with number {createBuyOrder.TransactionNumber} allready exists for this account");
                return StatusCode(StatusCodes.Status409Conflict, new ErrorResult { Code = "duplicate_buy_order", Message = $"Buyorder with number {createBuyOrder.TransactionNumber} for this account allready exists" });
            }

            BuyOrder buyOrder = new BuyOrder(createBuyOrder.Id, createBuyOrder.AccountId, createBuyOrder.Symbol, createBuyOrder.Currency, createBuyOrder.TransactionDate, createBuyOrder.TransactionNumber, userId.Value);

            await _TransactionsDbContext.BuyOrders.AddAsync(buyOrder);

            await _TransactionsDbContext.SaveChangesAsync(cancellationToken);

            _Logger.LogInformation($"Created Buy Order with Id: {createBuyOrder.Id} and number {createBuyOrder.TransactionNumber}");
            return Ok();
        }

        [HttpPut("{transactionId}/amount")]
        public async Task<IActionResult> Amount(Guid transactionId, [FromBody] UpdateAmountCommand updateAmount, CancellationToken cancellationToken)
        {

            Claim userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);



            BuyOrder buyOrder = _TransactionsDbContext.BuyOrders.SingleOrDefault(bo => bo.Id == transactionId && bo.UserId == userId.Value);

            if (buyOrder == null)
            {
                _Logger.LogError($"Buyorder with id: {transactionId} found for user: {userId.Value}");

                return StatusCode(StatusCodes.Status404NotFound);
            }

            _AccountService.Authenticate(_JwtTokenBuilder.Build(this.User));
            AccountInfo account = await _AccountService.GetAsync(buyOrder.AccountId);



            decimal exchangerate = updateAmount.Exchangerate;

            if (string.Equals(account.Currency, buyOrder.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                exchangerate = 1.00m;
            };



            buyOrder.UpdateAmount(updateAmount.Quantity, updateAmount.Price, exchangerate);


            await _TransactionsDbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPut("{transactionId}/costs")]
        public async Task<IActionResult> Costs(Guid transactionId, [FromBody] UpdateCostsCommand updateCosts, CancellationToken cancellationToken)
        {
            Claim userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            BuyOrder buyOrder = _TransactionsDbContext.BuyOrders.Include(b => b.Costs).SingleOrDefault(bo => bo.Id == transactionId && bo.UserId == userId.Value);

            if (buyOrder == null)
            {
                _Logger.LogError($"Buyorder with id: {transactionId} not found for user: {userId.Value}");

                return StatusCode(StatusCodes.Status404NotFound);
            }


            Cost commission = new Cost(updateCosts.Commision, "Commission");
            buyOrder.With(commission);

            Cost stockMarketTax = new Cost(updateCosts.StockMarketTax, "Stock market tax");
            buyOrder.With(stockMarketTax);

            _AccountService.Authenticate(_JwtTokenBuilder.Build(this.User));
            AccountInfo account = await _AccountService.GetAsync(buyOrder.AccountId);

            if (string.Equals(account.Currency, buyOrder.Currency, StringComparison.InvariantCultureIgnoreCase))
            {
                Cost costExchangerate = new Cost(updateCosts.CostExchangerate, "Exchange rate cost");

                buyOrder.With(costExchangerate);
            }

            await _TransactionsDbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }


        [HttpGet("{transactionId}/amount")]
        public async Task<IActionResult> Amount(Guid transactionId)
        {
            var buyOrder = _TransactionsDbContext.BuyOrders.Where(bo => bo.Id == transactionId).Single();

            _AccountService.Authenticate(_JwtTokenBuilder.Build(this.User));

             AccountInfo account = await _AccountService.GetAsync(buyOrder.AccountId);

            var result = new
            {
                Quantity = buyOrder.Quantity,
                Price = buyOrder.Price.ToString("0.00"),
                PriceCurrency = buyOrder.Currency,
                ExchangeRate = buyOrder.ExchangeRate.ToString("# ##0.00000"),
                Currency = account.Currency,
                Total = buyOrder.Subtotal.ToString("# ##0.00")
            };

            return Ok(result);
        }

        [HttpGet("{transactionId}/costs")]
        public async Task<IActionResult> Costs(Guid transactionId)
        {
            BuyOrder buyOrder = _TransactionsDbContext.BuyOrders.Where(bo => bo.Id == transactionId)
                                                                .Include(bo => bo.Costs)
                                                                .Single();
            _AccountService.Authenticate(_JwtTokenBuilder.Build(this.User));

            AccountInfo account = await _AccountService.GetAsync(buyOrder.AccountId);

            var result = new
            {
                Commission = buyOrder.Costs.Single(x => x.Type == "Commission").Amount.ToString("# ##0.00"),
                CostExchangeRate = buyOrder.Costs.Single(x => x.Type == "Stock market tax").Amount.ToString("# ##0.00"),
                StockMarketTax = buyOrder.Costs.Single(x => x.Type == "Exchange rate cost").Amount.ToString("# ##0.00"),
                Total = buyOrder.TotalCosts.ToString("# ##0.00"),
                Currency = account.Currency
            };

            return Ok(result);
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> Overview(Guid transactionId)
        {
            BuyOrder buyOrder = _TransactionsDbContext.BuyOrders.Where(bo => bo.Id == transactionId)
                                                                .Single();

            _AccountService.Authenticate(_JwtTokenBuilder.Build(this.User));
            
            AccountInfo account = await _AccountService.GetAsync(buyOrder.AccountId);

            var result = new {
                TransactionNumber = buyOrder.Number,
                Symbol = buyOrder.Symbol,
                TransactionDate = buyOrder.Date.ToString("dd/MM/yyyy"),
                SelectedAccount = account.Name,
                Id = buyOrder.Id,
                Currency = buyOrder.Currency,
            };

            return Ok(result);
        }
    }
}
