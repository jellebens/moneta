using System;

namespace AccountService.Domain
{
    public class Deposit
    {
        public Guid Id { get; set; }
        public int Year { get; set; }

        public decimal Amount { get; set; }

        public Account Account { get; set; }

    }
}
