using FluentValidation;
using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Validation.InvoiceLineValidation
{
    public class InvoiceLineValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineValidator()
        {
            Include(new InvoiceLineKDVValidator());
            Include(new InvoiceLineOIVValidator());
            Include(new InvoiceLineOTVValidator());
            Include(new InvoiceLineRowNumberValidator());
        }
    }
}
