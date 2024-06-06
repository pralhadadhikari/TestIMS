using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class ReportDetailViewModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CustomerName { get; set; }        
        public int PaymentMethod { get; set; }        
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string UnitName { get; set; }
        public double Amount { get; set; }
        public string SupplierName { get; set; }

    }
}
