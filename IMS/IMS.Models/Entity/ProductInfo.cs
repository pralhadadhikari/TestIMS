using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class ProductInfo:BaseEntity
    {
        public int CategoryInfoId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get;set; }
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Unit")]
        public int UnitInfoId { get; set; }        
        public int StoreInfoId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get;set; }
        public string ImageUrl { get;set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual CategoryInfo CategoryInfo { get; set; }
        public virtual UnitInfo UnitInfo { get; set; }
        public virtual StoreInfo StoreInfo { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get; }
        public virtual ICollection<StockInfo> StockInfos { get; set; }
        public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }

    }
}
