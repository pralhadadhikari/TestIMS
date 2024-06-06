using IMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class StockViewModel
    {
        public StockInfo StockInfo { get; set; }
        public IEnumerable<StockInfo> StockInfos { get; set; }
        public int? categoryId { get; set; }
        public int? productId { get; set; }
    }
}
