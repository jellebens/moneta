using System;

namespace AccountService.Contracts.Data
{
    public class AccountSummary
    {
        public  Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public decimal Total { get; set; }

        public SummaryLine[] Lines { get; set; }
    }

    public class SummaryLine
    {
        public int Year { get; set; }

        public decimal Amount { get; set; }
    }
}
