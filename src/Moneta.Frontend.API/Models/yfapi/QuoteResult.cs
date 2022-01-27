using System.Collections.Generic;

namespace Moneta.Frontend.API.Models.Yfapi
{
    

    public class QuoteResult
    {
        public string Language { get; set; }
        public string Region { get; set; }
        public string QuoteType { get; set; }
        public string QuoteSourceName { get; set; }
        public bool Triggerable { get; set; }
        public string MarketState { get; set; }
        public string Currency { get; set; }
        public long FirstTradeDateMilliseconds { get; set; }
        public int PriceHint { get; set; }
        public decimal RegularMarketChange { get; set; }
        public decimal RegularMarketChangePercent { get; set; }
        public int RegularMarketTime { get; set; }
        public decimal RegularMarketPrice { get; set; }
        public decimal RegularMarketDayHigh { get; set; }
        public string RegularMarketDayRange { get; set; }
        public decimal RegularMarketDayLow { get; set; }
        public int RegularMarketVolume { get; set; }
        public decimal RegularMarketPreviousClose { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public string FullExchangeName { get; set; }
        public decimal RegularMarketOpen { get; set; }
        public int AverageDailyVolume3Month { get; set; }
        public int AverageDailyVolume10Day { get; set; }
        public decimal FiftyTwoWeekLowChange { get; set; }
        public decimal FiftyTwoWeekLowChangePercent { get; set; }
        public string FiftyTwoWeekRange { get; set; }
        public decimal FiftyTwoWeekHighChange { get; set; }
        public decimal FiftyTwoWeekHighChangePercent { get; set; }
        public decimal FiftyTwoWeekLow { get; set; }
        public decimal FiftyTwoWeekHigh { get; set; }
        public string Exchange { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string ExchangeTimezoneName { get; set; }
        public string ExchangeTimezoneShortName { get; set; }
        public int GmtOffSetMilliseconds { get; set; }
        public string Market { get; set; }
        public bool EsgPopulated { get; set; }
        public decimal YtdReturn { get; set; }
        public int TrailingThreeMonthReturns { get; set; }
        public decimal TrailingThreeMonthNavReturns { get; set; }
        public decimal FiftyDayAverage { get; set; }
        public decimal FiftyDayAverageChange { get; set; }
        public decimal FiftyDayAverageChangePercent { get; set; }
        public decimal TwoHundredDayAverage { get; set; }
        public decimal TwoHundredDayAverageChange { get; set; }
        public decimal TwoHundredDayAverageChangePercent { get; set; }
        public int SourceInterval { get; set; }
        public int ExchangeDataDelayedBy { get; set; }
        public bool Tradeable { get; set; }
        public string Symbol { get; set; }
    }

    public class QuoteResponse
    {
        public List<QuoteResult> Result { get; set; }
        public object Error { get; set; }
    }

    public class QuoteResults
    {
        public QuoteResponse QuoteResponse { get; set; }
    }

}
