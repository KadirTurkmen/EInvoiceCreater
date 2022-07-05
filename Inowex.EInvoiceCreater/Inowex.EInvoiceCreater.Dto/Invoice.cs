using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Dto
{
    public class Invoice
    {
        public ProfileIds ScenarioType { get; set; }
        public InvoiceTypes InvioceType { get; set; }
        public Guid Guid { get; set; }
        public string No { get; set; }
        public string[] Notes { get; set; }
        public DateTime Date { get; set; }
        public string CurrencyIso4217Code { get; set; }
        public CustomerInformation CustomerInfo { get; set; }
        public SupplierInformation SupplierInfo { get; set; }
        public byte[] Xslt { get; set; }
        public bool CopyIndicator { get; set; }
        public List<InvoiceLine> InvoiceLine { get; set; } = new List<InvoiceLine>();
    }
}
