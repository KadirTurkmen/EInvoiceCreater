using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Dto
{
    public class Tax
    {
        public decimal Percent { get; set; }
        public decimal Amount { get; set; }
        public string? TaxCode { get; set; }
        public string? TaxName { get; set; }
    }
}
