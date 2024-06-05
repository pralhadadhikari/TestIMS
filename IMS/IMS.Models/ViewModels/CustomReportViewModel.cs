using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class CustomReportViewModel
    {
        //public int Id { get; set; }
        public int TransactionId { get; set; }
        public DateTime TransactionDate {  get; set; }
        public string CustomerName { get; set; }
        public double NetAmount {  get; set; }
        public double DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public int PaymentMethod { get; set;}
        public string Remarks { get; set; }
       
    }
}
