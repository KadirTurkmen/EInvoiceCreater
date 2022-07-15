using FluentValidation;
using Inowex.EInvoiceCreater.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.CompanyInformationValidation
{
    internal class CompanyInformationValidator : AbstractValidator<CompanyInformation>
    {
        public CompanyInformationValidator()
        {
            Include(new CompanyInformationTaxValidator());
            Include(new CompanyInformationAddressValidator());
        }
    }
}
