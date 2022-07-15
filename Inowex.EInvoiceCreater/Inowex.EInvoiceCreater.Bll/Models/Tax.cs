using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Models
{
    public class Tax
    {
        /// <summary>
        /// Vergi oranı
        /// </summary>
        public decimal Percent { get; set; }
        /// <summary>
        /// Vergi tutarı
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Gib'de tanımlı vergi kodu
        /// </summary>
        public string? TaxCode { get; set; }
        /// <summary>
        /// Vergi adı
        /// </summary>
        public string? TaxName { get; set; }
    }
}
