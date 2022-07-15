using FluentValidation;
using Inowex.EInvoiceCreater.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceLineValidation
{
    public class InvoiceLineKDVValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineKDVValidator()
        {
            RuleFor(x => x.KDVAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.KDVPercent).InclusiveBetween(1, 100).When(x => x.KDVAmount > 0);
            RuleFor(x => x.KDVPercent).Equal(0).When(x => x.KDVAmount == 0);
        }
    }
}
