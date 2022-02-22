﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Commands.Instruments
{
    public class CreateInstrumentCommand : ICommand
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
