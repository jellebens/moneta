namespace Moneta.Frontend.API.Models.Instruments
{
    public class InstrumentSearchResult
    {
        public string Exchange { get; internal set; }
        public string Symbol { get; internal set; }
        public string Type { get; internal set; }
        public string Name { get; internal set; }
    }
}
