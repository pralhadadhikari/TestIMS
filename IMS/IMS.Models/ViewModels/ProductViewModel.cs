using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int CategoryInfoId { get; set; }
        
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        
        [Display(Name = "Unit")]
        public int UnitInfoId { get; set; }
        public int StoreInfoId { get; set; }        
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        
    }
}
