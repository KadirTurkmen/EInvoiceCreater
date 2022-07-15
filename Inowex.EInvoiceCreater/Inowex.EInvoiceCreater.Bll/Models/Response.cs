using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inowex.EInvoiceCreater.Bll.Models
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
        public T? Data { get; set; }
        /// <summary>
        /// İşlem sırasında meydana gelen hataları belirtir
        /// </summary>
        public List<Error>? Errors { get; set; }
    }

    public class Error
    {
        /// <summary>
        /// Hata kodudur
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Hatanın yer aldığı class adıdır
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// Hata Açıklamasıdır
        /// </summary>
        public string? Message { get; set; }
    }
}
