using FluentValidation;
using Inowex.EInvoiceCreater.Bll.Validation.CompanyInformationValidation;
using Inowex.EInvoiceCreater.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.SupplierInformationValidation
{
    public class SupplierInformationValidator : AbstractValidator<SupplierInformation>
    {
        public SupplierInformationValidator()
        {
            RuleFor(c => c).NotNull();
            Include(new CompanyInformationTaxValidator());
            Include(new CompanyInformationAddressValidator());
        }
    }
}
