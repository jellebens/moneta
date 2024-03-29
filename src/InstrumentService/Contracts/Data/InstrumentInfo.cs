﻿namespace InstrumentService.Contracts.Data
{
    public class InstrumentInfo
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public string Currency { get; set; }
        public Guid Id { get; internal set; }
    }
}
