﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain
{
    public class BuyOrder
    {
        protected BuyOrder()
        {

        }

        public BuyOrder(Guid id, Guid accountId, string symbol, string currency, DateTime transactionDate, int transactionNumber, string userId)
        {
            Id = id;
            AccountId = accountId;
            UserId = userId;
            Symbol = symbol;
            Date = transactionDate;
            Number = transactionNumber;
            Currency = currency;
        }

        public Guid Id { get; protected set; }

        public Guid AccountId { get; protected set; }

        public string UserId { get; protected set; }

        public string Symbol { get; protected set; }

        public long Number { get; protected set; }

        public DateTime Date { get; protected set; }

        public int Quantity { get; protected set; }
        public decimal Price { get; protected set; }
        public decimal ExchangeRate { get; protected set; }

        public decimal Subtotal
        {
            get
            {
               
                return this.Quantity * this.Price / this.ExchangeRate;

            }
        }

        public decimal TotalCosts
        {
            get
            {
                decimal costs = 0;
                if (this.Costs == null) {
                    return costs;
                }

                foreach (Cost cost in Costs)
                {
                    costs += cost.Amount;
                }

                return costs;
            }
        }

        public IList<Cost> Costs { get; internal set; }
        public string Currency { get; protected set; }

        internal void With(Cost cost)
        {
            if (this.Costs == null) {
                this.Costs = new List<Cost>();
            }

            Cost c = this.Costs.SingleOrDefault(c => c.Type == cost.Type);

            if (c != null)
            {
                c.Amount = cost.Amount;
            }
            else {
                this.Costs.Add(cost);
            }
        }

        public void UpdateAmount(int quantity, decimal price, decimal exchangerate)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.ExchangeRate = exchangerate;
        }
    }
}
