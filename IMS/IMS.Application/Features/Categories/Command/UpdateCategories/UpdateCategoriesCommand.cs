using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Command.UpdateCategories
{
    public class UpdateCategoriesCommand:IRequest<CategoryInfo>
    {
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string CategoryDescription { get; set; }

        [Required]
        public int StoreInfoId { get; set; }

        public bool IsActive { get; set; }
    }
}
