using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class RackInfo:BaseEntity
    {
        [Required]
        [Display(Name = "Rack Name")]
        public string RackName { get; set; }
        public int StoreInfoId { get;set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<ProductRateInfo> ProductRateInfos { get; }
        public virtual StoreInfo StoreInfo { get; set; }
    }
}
