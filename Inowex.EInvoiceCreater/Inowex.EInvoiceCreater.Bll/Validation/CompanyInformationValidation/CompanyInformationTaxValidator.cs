using FluentValidation;
using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.CompanyInformationValidation
{
    public class CompanyInformationTaxValidator : AbstractValidator<CompanyInformation>
    {
        public CompanyInformationTaxValidator()
        {
            RuleFor(x => x.TaxNumber).NotNull();
            RuleFor(x => x.TaxNumber).MinimumLength(10).MaximumLength(11);
            RuleFor(x => x.TaxOffice).NotNull();
        }
    }
}
