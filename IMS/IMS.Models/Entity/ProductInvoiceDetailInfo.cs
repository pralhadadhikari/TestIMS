using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class ProductInvoiceDetailInfo:BaseEntity
    {
        public int ProductInvoiceInfoId { get; set; }
        public int ProductRateInfoId { get; set; }
        public float Rate { get;set; }
        public float Quantity { get;set; }
        public float Amount { get;set; }        
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }    
        public virtual ProductInvoiceInfo ProductInvoiceInfo { get; set; }
        public virtual ProductRateInfo ProductRateInfo { get; set; }
    }
}
