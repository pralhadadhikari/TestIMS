using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.Entity
{
    public class StoreInfo:BaseEntity
    {
        public string StoreName { get;set;}
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNo { get; set; }
        public string PanNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
