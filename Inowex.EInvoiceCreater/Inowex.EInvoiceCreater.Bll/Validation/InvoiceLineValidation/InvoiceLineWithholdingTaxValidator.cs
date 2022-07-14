using FluentValidation;
using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceLineValidation
{
    public class InvoiceLineWithholdingTaxValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineWithholdingTaxValidator()
        {
            RuleFor(x => x.WithholdingTaxAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.WithholdingTaxPercent).InclusiveBetween(1, 100).When(x => x.WithholdingTaxAmount > 0);
            RuleFor(x => x.WithholdingTaxPercent).Equal(0).When(x => x.WithholdingTaxAmount == 0);
        }
    }
}
