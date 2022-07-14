using FluentValidation;
using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceLineValidation
{
    public class InvoiceLineOIVValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineOIVValidator()
        {
            RuleFor(x => x.OIVAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.OIVPercent).InclusiveBetween(1, 100).When(x => x.OIVAmount > 0);
            RuleFor(x => x.OIVPercent).Equal(0).When(x => x.OIVAmount == 0);
        }
    }
}
