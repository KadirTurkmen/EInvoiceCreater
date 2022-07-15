using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Models
{
    /// <summary>
    /// Firma bilgileri
    /// </summary>
    public class CompanyInformation
    {
        /// <summary>
        /// Firma blok bilgisi
        /// </summary>
        public string? BlockName { get; set; }

        /// <summary>
        /// Firma bina bilgisi
        /// </summary>
        [Required(ErrorMessage = "BuildingName is required")]
        public string? BuildingName { get; set; }

        /// <summary>
        /// Firma bina numarası
        /// </summary>
        [Required(ErrorMessage = "BuildingNumber is required")]
        public string? BuildingNumber { get; set; }

        /// <summary>
        /// Firma bulunduğu şehir
        /// </summary>
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        /// <summary>
        /// Firma bulunduğu ilçe
        /// </summary>
        [Required(ErrorMessage = "CitySubdivision is required")]
        public string? CitySubdivision { get; set; }

        /// <summary>
        /// Firma bulunduğu ülke
        /// </summary>
        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

        /// <summary>
        /// Firma'ya ait vergi kimlik numarası adı altında tanımlanmış olan alias kodudur
        /// </summary>
        [Required(ErrorMessage = "EmailTag is required")]
        public string? EmailTag { get; set; }

        /// <summary>
        /// Firma mail bilgisi
        /// </summary>
        public string? Mail { get; set; }

        /// <summary>
        /// Firma mersis numarası
        /// </summary>
        public string? MersisNo { get; set; }

        /// <summary>
        /// Firma unvan veya isim bilgisi
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        /// <summary>
        /// Firma telefon numarası
        /// </summary>
        [Required(ErrorMessage = "Phone is required")]
        public string? Phone { get; set; }

        /// <summary>
        /// Firma posta kodu
        /// </summary>
        [Required(ErrorMessage = "PostalZone is required")]
        public string? PostalZone { get; set; }

        /// <summary>
        /// Firma daire bilgisi
        /// </summary>
        [Required(ErrorMessage = "Room is required")]
        public string? Room { get; set; }

        /// <summary>
        /// Firma vergi numarası
        /// </summary>
        [Required(ErrorMessage = "TaxNumber is required")]
        [MaxLength(11),MinLength(10)]
        public string? TaxNumber { get; set; }

        /// <summary>
        /// Firma vergi dairesi
        /// </summary>
        [Required(ErrorMessage = "TaxOffice is required")]
        public string? TaxOffice { get; set; }

        /// <summary>
        /// Firma web site adresi
        /// </summary>
        public string? WebSiteUri { get; set; }
    }
}
