using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class DashboardIndex
    {
        public double TotalTransaction { get;set; }
        public int Sucessful { get; set; }
        public int Canelled { get; set; }
        public int Completed { get; set; }

    }
}
