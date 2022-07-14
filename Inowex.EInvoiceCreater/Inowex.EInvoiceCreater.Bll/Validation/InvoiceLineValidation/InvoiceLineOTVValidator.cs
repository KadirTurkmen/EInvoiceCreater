using FluentValidation;
using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceLineValidation
{
    public class InvoiceLineOTVValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineOTVValidator()
        {
            RuleFor(x => x.OTVAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.OTVPercent).InclusiveBetween(1, 100).When(x => x.OTVAmount > 0);
            RuleFor(x => x.OTVPercent).Equal(0).When(x => x.OTVAmount == 0);
        }
    }
}
