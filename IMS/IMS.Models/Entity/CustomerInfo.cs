using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class CustomerInfo:BaseEntity
    {
        public int StoreInfoId { get;set; }
        public string CustomerName {  get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get;set; }
        public string PanNo { get;set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public virtual StoreInfo StoreInfo { get; set; }
        public virtual ICollection<ProductInvoiceInfo> ProductInvoiceInfos { get; set; }

    }
}
