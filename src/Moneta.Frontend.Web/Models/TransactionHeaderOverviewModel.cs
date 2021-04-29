using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionHeaderOverviewModel
    {
        
        public int? TransactionNumber { get; set; }
        
        public string Symbol { get; set; }
        
        public string TransactionDate { get; set; }

        public string SelectedAccount { get; set; }

        public string Currency { get; set; }
    }
}
