using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class ImportTransactionModel
    {
        [Required]
        public string Lines { get; set; }
    }
}
