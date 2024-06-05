using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class SearchCriteria
    {
        public int? CustomerId { get;set; }
        public int? SupplierId { get;set; }
        public int? PaymentMethod { get;set; }
        public DateTime? StartDate { get;set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId { get;set; }
        public int? ProductId { get; set; }
    }
}
