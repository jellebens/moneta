using System;

namespace TransactionService.Contracts.Data
{
    public class DepositCommand
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

    }
}
