using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Dto
{
    public class Response<T>
    {
        /// <summary>
        /// İşlem açıklamasını barındırır
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// İşlem başarılı ise True başarısız ise False değerini alır
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// İşlem sonrasında dönecek olan değer veya değerleri barındırır
        /// </summary>
        public T? Result { get; set; }
    }
}
