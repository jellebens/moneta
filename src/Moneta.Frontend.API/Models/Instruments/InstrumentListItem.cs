using System;

namespace Moneta.Frontend.API.Models.Instruments
{
    public class InstrumentListItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public string Currency { get; set; }
    }
}
