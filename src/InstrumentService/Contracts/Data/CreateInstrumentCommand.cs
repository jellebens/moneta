namespace InstrumentService.Contracts.Data
{
    public class CreateInstrumentCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Currency { get; set; }

        public int Sector { get; set; }

        public string Symbol { get; set; }

        public string Isin { get; set; }

        public string Type { get; set; }

        public string Exchange { get; set; }
    }
}
