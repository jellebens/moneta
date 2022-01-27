using System.Collections.Generic;

namespace Moneta.Frontend.API.Models.Yfapi
{
    public class AutoCompleteResult
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Exch { get; set; }
        public string Type { get; set; }
        public string ExchDisp { get; set; }
        public string TypeDisp { get; set; }
    }

    public class ResultSet
    {
        public string Query { get; set; }
        public List<AutoCompleteResult> Result { get; set; }
    }

    public class AutoCompleteResponse
    {
        public ResultSet ResultSet { get; set; }
    }
}
