using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionHeaderModel
    {
        [Required(ErrorMessage = "Transaction Number is required")]
        public int? TransactionNumber { get; set; }

        [Required(ErrorMessage = "Transaction Symbol is required")]
        public string Symbol { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        public string TransactionDate { get; set; }

        [Required(ErrorMessage = "Account is required")]
        public Guid? SelectedAccount { get; set; }

        [Required(ErrorMessage = "Account Currency is required")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Id is required")]
        public Guid? Id { get; set; }
    }
}
