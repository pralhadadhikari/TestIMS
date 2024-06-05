using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class StoreInfo:BaseEntity
    {
        [Required(ErrorMessage ="Please Input Store Name")]
        
        [Display(Name ="*Store Name")]
        public string StoreName { get;set;}
        [Required]
        public string Address { get; set; }
        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public string RegistrationNo { get; set; }
        [Required]
        public string PanNo { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<CategoryInfo> CategoryInfos { get; set; }
        public virtual ICollection<CustomerInfo> CustomerInfos { get; set; }
        public virtual ICollection<ProductInfo> ProductInfos { get; set; }
        public virtual ICollection<ProductInvoiceInfo> ProductInvoiceInfos { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get; }
        public virtual ICollection<RackInfo> RackInfos { get; set; }
        public virtual ICollection<StockInfo> StockInfos { get; set; }
        public virtual ICollection<SupplierInfo> SupplierInfos { get; set; }
        public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }
    }
}
