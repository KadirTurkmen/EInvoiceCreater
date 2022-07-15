using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Models
{
    public class Invoice
    {
        /// <summary>
        /// Fatura profil bilgisidir
        /// </summary>
        [Required(ErrorMessage = "ScenarioType is required")]
        public ProfileIds ScenarioType { get; set; }
        /// <summary>
        /// Fatura senaryo bilgisidir
        /// </summary>
        [Required(ErrorMessage = "InvioceType is required")]
        public InvoiceTypes InvioceType { get; set; }
        /// <summary>
        /// Faturadaki ETTN bilgisidir
        /// </summary>
        [Required(ErrorMessage = "Guid is required")]
        public Guid Guid { get; set; }
        /// <summary>
        /// Fatura numarasıdır
        /// </summary>
        [Required(ErrorMessage = "No is required")]
        [StringLength(16)]
        public string? No { get; set; }
        /// <summary>
        /// Fatura dipnot bilgileridir
        /// </summary>
        public string[]? Notes { get; set; }
        /// <summary>
        /// Fatura tarihi bilgisidir
        /// </summary>
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        /// <summary>
        /// Fatura tutarının ISO formatindaki para birimi kodu
        /// </summary>
        [Required(ErrorMessage = "CurrencyIso4217Code is required")]
        public string? CurrencyIso4217Code { get; set; }
        /// <summary>
        /// Müşteri bilgileridir
        /// </summary>
        [Required(ErrorMessage = "CustomerInfo is required")]
        public CustomerInformation CustomerInfo { get; set; } = new CustomerInformation();
        /// <summary>
        /// Tedarikçi bilgileridir
        /// </summary>
        [Required(ErrorMessage = "SupplierInfo is required")]
        public SupplierInformation SupplierInfo { get; set; } = new SupplierInformation();
        /// <summary>
        /// Fatura şablonuna ait XSLT'dir
        /// </summary>
        [Required(ErrorMessage = "Xslt is required")]
        public byte[]? Xslt { get; set; }
        /// <summary>
        /// Faturanın asıl veya suret olduğu bilgisidir. Asıl false, suret true
        /// </summary>
        [Required(ErrorMessage = "CopyIndicator is required")]
        public bool CopyIndicator { get; set; }
        /// <summary>
        /// Faturadaki ürün bilgileridir
        /// </summary>
        [Required(ErrorMessage = "InvoiceLine is required")]
        public List<InvoiceLine> InvoiceLine { get; set; } = new List<InvoiceLine>();
    }
}
