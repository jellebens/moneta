using System.Collections.Generic;

namespace Moneta.Frontend.API.Models.rapidapi
{
        public class Quote
        {
            public string Exchange { get; set; }
            public string Shortname { get; set; }
            public string QuoteType { get; set; }
            public string Symbol { get; set; }
            public string Index { get; set; }
            public double Score { get; set; }
            public string TypeDisp { get; set; }
            public string Longname { get; set; }
            public string ExchDisp { get; set; }
            public bool IsYahooFinance { get; set; }
        }

        public class AutoCompleteResponse
        {
            public int Count { get; set; }
            public List<Quote> Quotes { get; set; }
        }
    
}
