using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class CategoryInfo:BaseEntity
    {
        [Required]
        [Display(Name ="Category")]
        public string CategoryName {  get; set; }
        [Required]
        [Display(Name = "Description")]
        public string CategoryDescription { get; set; }
        public int StoreInfoId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }        
        public virtual StoreInfo StoreInfo { get; set; } 
        public virtual ICollection<ProductInfo> ProductInfos { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get;}
        public virtual ICollection<StockInfo> StockInfos { get; set; }
        public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }
    }
}
