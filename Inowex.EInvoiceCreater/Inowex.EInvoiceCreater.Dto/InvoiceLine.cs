using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Dto
{
    public class InvoiceLine
    {
        /// <summary>
        /// Fatura kalemindeki ürüne ait miktar bilgisi
        /// </summary>
        [Required(ErrorMessage = "Quantity is required")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Fatura kalemine ait iskonto arttırım veya azaltım oranı bilgisidir.
        /// </summary>
        public decimal DiscountPercent { get; set; }
        /// <summary>
        /// Fatura kalemine ait iskonto arttırım veya azaltım tutarı bilgisidir.
        /// Azaltım için - (eksi) değer gönderiniz, Arttırım için + (artı) değer gönderiniz.
        /// </summary>
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// Fatura kalemi toplam tutarı
        /// </summary>
        [Required(ErrorMessage = "LineTotalAmount is required")]
        public decimal LineTotalAmount { get; set; }
        /// <summary>
        /// Kdv oranı
        /// </summary>
        public decimal KDVPercent { get; set; }
        /// <summary>
        /// Kdv tutarı
        /// </summary>
        public decimal KDVAmount { get; set; }
        /// <summary>
        /// Fatura kalemine ait note
        /// </summary>
        public string? Note { get; set; }
        /// <summary>
        /// OİV oranı
        /// </summary>
        public decimal OIVPercent { get; set; }
        /// <summary>
        /// OİV tutarı
        /// </summary>
        public decimal OIVAmount { get; set; }
        /// <summary>
        /// ÖTV oranı
        /// </summary>
        public decimal OTVPercent { get; set; }
        /// <summary>
        /// ÖTV tutarı
        /// </summary>
        public decimal OTVAmount { get; set; }
        /// <summary>
        /// Fatura kalemine ait sıra numarası
        /// </summary>
        [Required(ErrorMessage = "RowNumber is required")]
        public int RowNumber { get; set; }
        /// <summary>
        /// Stok kodu
        /// </summary>
        public string? StockCode { get; set; }
        /// <summary>
        /// Stok adı
        /// </summary>
        public string? StockName { get; set; }
        /// <summary>
        /// Fatura kalemindeki ürün birimine ait ISO kodu
        /// </summary>
        [Required(ErrorMessage = "UnitIsoCode is required")]
        public string? UnitIsoCode { get; set; }
        /// <summary>
        /// Birim fiyat
        /// </summary>
        [Required(ErrorMessage = "UnitPrice is required")]
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Fatura kalemindeki KDV ye uygulanan tevkifat kodu. Gib'de tanımlı tevkifat kodlarından olmalıdır.
        /// </summary>
        public string? WithholdingTaxCode { get; set; }
        /// <summary>
        /// Fatura kalemindeki KDV ye uygulanan tevkifat adıdır.
        /// </summary>
        public string? WithholdingTaxName { get; set; }
        /// <summary>
        /// Fatura kalemindeki KDV ye uygulanan tevkifat oranıdır.
        /// </summary>
        public decimal WithholdingTaxPercent { get; set; }
        /// <summary>
        /// Fatura kalemindeki KDV ye uygulanan tevkifat tutarıdır.
        /// </summary>
        public decimal WithholdingTaxAmount { get; set; }
    }
}
