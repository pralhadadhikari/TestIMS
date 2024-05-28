using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class UnitInfo:BaseEntity
    {
        [Required]

        [Display(Name = "Unit Name *")]
        public string UnitName { get;set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<ProductInfo> ProductInfos { get; set; }
        public virtual ICollection<TransactionInfo> TransactionInfos { get; set; }
    }
}
