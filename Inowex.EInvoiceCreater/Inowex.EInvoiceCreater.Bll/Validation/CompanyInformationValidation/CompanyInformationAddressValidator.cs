using FluentValidation;
using Inowex.EInvoiceCreater.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.CompanyInformationValidation
{
    public class CompanyInformationAddressValidator : AbstractValidator<CompanyInformation>
    {
        public CompanyInformationAddressValidator()
        {
            RuleFor(x => x.BuildingName).NotNull();
            RuleFor(x => x.BuildingNumber).NotNull();
            RuleFor(x => x.City).NotNull();
            RuleFor(x => x.CitySubdivision).NotNull();
            RuleFor(x => x.Country).NotNull();
            RuleFor(x => x.EmailTag).NotNull();
            RuleFor(x => x.EmailTag).EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Phone).NotNull();
            RuleFor(x => x.PostalZone).NotNull();
            RuleFor(x => x.Room).NotNull();
        }
    }
}
