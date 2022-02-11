namespace InstrumentService.Domain
{
    public class Instrument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Isin { get; set; }

        public string Symbol { get; set; }

        public Currency Currency { get; set; }

        public bool IsDeleted { get; set; }

        public string Exchange { get; set; }

        public Sector Sector { get; set; }
    }
}
