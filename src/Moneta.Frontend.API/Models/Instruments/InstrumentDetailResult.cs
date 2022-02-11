namespace Moneta.Frontend.API.Models.Instruments
{
    public class InstrumentDetailResult
    {
        public string Exchange { get; internal set; }
        public string Symbol { get; internal set; }
        public string Type { get; internal set; }
        public string Name { get; internal set; }

        public string Currency { get; set; }
    }
}
