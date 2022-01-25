namespace InstrumentService.Domain
{
    public class Instrument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Isin { get; set; }

        public string Ticker { get; set; }

        public Currency Currency { get; set; }

        public string Url { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}
