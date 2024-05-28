using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class ProductRateInfo:BaseEntity
    {
        [Required]
        [Display(Name = "Category")]
        public int CategoryInfoId { get; set; }
        [Required]
        [Display(Name = "Product")]
        public int ProductInfoId { get; set; }
        public int StoreInfoId { get; set; }
        [Required]
        [Display(Name = "Cost Price")]
        public float CostPrice { get; set; }
        [NotMapped]
       
        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        [Required]
        [Display(Name = "Selling Price")]
        public float SellingPrice { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public float Quantity { get; set; }
       
        [Display(Name = "Unit")]
        public float SoldQuantity { get; set; }
        
        [Display(Name = "Remaining Quantity")]
        public float RemainingQuantity { get; set; }
        [Required]
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public DateTime PurchasedDate { get; set; }
        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime? Expirydate { get; set; }
        [Required]
        [Display(Name = "Supplier")]
        public int SupplierInfoId { get; set; }
        [Required]
        [Display(Name = "Rack No")]
        public int RackInfoId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<ProductInvoiceDetailInfo> ProductInvoiceDetailInfos { get; set; }
        public virtual CategoryInfo CategoryInfo { get; set; }
        public virtual ProductInfo ProductInfo { get;set; }
        public virtual StoreInfo StoreInfo { get; set; }
        public virtual SupplierInfo SupplierInfo { get; set; }
        public virtual RackInfo RackInfo { get; set; }
        public virtual ICollection<StockInfo> StockInfos { get; set; }
        public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }
    }
}
