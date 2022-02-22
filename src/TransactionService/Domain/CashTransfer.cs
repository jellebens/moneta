using System;

namespace TransactionService.Domain
{
    public class CashTransfer
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }

        public Guid AccountId { get; set; }

        public Currency Currency { get; set; }

    }
}
