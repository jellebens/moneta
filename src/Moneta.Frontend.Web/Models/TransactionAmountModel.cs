using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionAmountModel
    {

        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Exchangerate is required")]
        public decimal Exchangerate { get; internal set; }
    }
}
