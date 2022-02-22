using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Events.Transactions
{
    public class CashWithdrawnEvent
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}
