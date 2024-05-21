using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class CustomerInfo:BaseEntity
    {
        public int StoreInfoId { get;set; }
        [Required]
        [Display(Name ="CustomerName Name")]
        public string CustomerName {  get; set; }  
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get;set; }
        public string PanNo { get;set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public virtual StoreInfo StoreInfo { get; set; }
        public virtual ICollection<ProductInvoiceInfo> ProductInvoiceInfos { get; set; }

    }
}
