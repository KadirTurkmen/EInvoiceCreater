using FluentValidation;
using Inowex.EInvoiceCreater.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceValidation
{
    public class InvoiceNoValidator : AbstractValidator<Invoice>
    {
        public InvoiceNoValidator()
        {
            RuleFor(x => x.No).Length(16);
        }
    }
}
