using FluentValidation;
using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceLineValidation
{
    public class InvoiceLineRowNumberValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineRowNumberValidator()
        {
            RuleFor(x => x.RowNumber).GreaterThan(0);
        }
    }
}
