using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Dto
{
    public class InvoiceLine
    {
        public decimal Quantity { get; set; }
        public string CurrencyIso4217Code { get; set; }
        public decimal DiscountPercent { get; set; }
        /// <summary>
        /// Fatura kalemine ait iskonto arttırım veya azaltım bilgisidir.
        /// Azaltım için - (eksi) değer gönderiniz, Arttırım için + (artı) değer gönderiniz.
        /// </summary>
        public decimal DiscountPrice { get; set; }
        public decimal LineTotalAmount { get; set; }
        public decimal KDVPercent { get; set; }
        public decimal KDVAmount { get; set; }
        public string Note { get; set; }
        public decimal OIVPercent { get; set; }
        public decimal OIVAmount { get; set; }
        public decimal OTVPercent { get; set; }
        public decimal OTVAmount { get; set; }
        public int RowNumber { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string UnitIsoCode { get; set; }
        public decimal UnitPrice { get; set; }
        public string WithholdingTaxCode { get; set; }
        public string WithholdingTaxName { get; set; }
        public decimal WithholdingTaxPercent { get; set; }
        public decimal WithholdingTaxAmount { get; set; }
    }
}
