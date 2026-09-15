using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Command.UpdateCategories
{
    public class UpdateCategoriesCommandHander : IRequestHandler<UpdateCategoriesCommand, CategoryInfo>
    {
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly IClaimedService _claimedService;

        public UpdateCategoriesCommandHander(ICrudService<CategoryInfo> categoryInfo, IClaimedService claimedService)
        {
            _categoryInfo = categoryInfo;
            _claimedService = claimedService;
        }
        public async Task<CategoryInfo> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryInfo.GetAsync(request.Id);

            if (category == null)
                return null;

            category.CategoryName = request.CategoryName;
            category.CategoryDescription = request.CategoryDescription;
            category.IsActive = request.IsActive;
            category.ModifiedDate = DateTime.Now;
            category.ModifiedBy = _claimedService.UserId;

            await _categoryInfo.UpdateAsync(category);

            return category;
        }
    }
}
