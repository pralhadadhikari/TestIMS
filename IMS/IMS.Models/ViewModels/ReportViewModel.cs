using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class ReportViewModel
    {
        public SearchCriteria SearchCriteria { get; set; }
        public IEnumerable<CustomReportViewModel> CustomReportViewModels { get; set;}
        public IEnumerable<ReportDetailViewModel> ReportDetailViewModels { get; set; }
    }
}
