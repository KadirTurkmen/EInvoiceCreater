using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Dto
{
    public class CompanyInformation
    {
        [Required(ErrorMessage = "BlockName is required")]
        public string? BlockName { get; set; }

        [Required(ErrorMessage = "BuildingName is required")]
        public string? BuildingName { get; set; }

        [Required(ErrorMessage = "BuildingNumber is required")]
        public string? BuildingNumber { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "CitySubdivision is required")]
        public string? CitySubdivision { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "EmailTag is required")]
        public string? EmailTag { get; set; }

        [Required(ErrorMessage = "Mail is required")]
        public string? Mail { get; set; }

        public string? MersisNo { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "PostalZone is required")]
        public string? PostalZone { get; set; }

        [Required(ErrorMessage = "Room is required")]
        public string? Room { get; set; }

        [Required(ErrorMessage = "TaxNumber is required")]
        public string? TaxNumber { get; set; }

        [Required(ErrorMessage = "TaxOffice is required")]
        public string? TaxOffice { get; set; }

        public string? WebSiteUri { get; set; }
    }
}
