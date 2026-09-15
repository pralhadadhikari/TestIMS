using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryInfo>
    {
        private readonly ICrudService<CategoryInfo> _categoryService;
        private readonly IClaimedService _claimedService;

        public CreateCategoryCommandHandler(ICrudService<CategoryInfo> categoryService, IClaimedService claimedService)
        {
            _categoryService = categoryService;
            _claimedService = claimedService;
        }
        public async Task<CategoryInfo> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                throw new ArgumentException("Category name is required.");

            if (string.IsNullOrWhiteSpace(request.CategoryDescription))
                throw new ArgumentException("Category description is required.");
           
            var userId = _claimedService.UserId;
            var role = _claimedService.Role;
            var storeId = await _claimedService.GetStoreIdAsync();

            var category = new CategoryInfo
            {
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription,
                StoreInfoId = storeId,
                IsActive = request.IsActive,
                CreatedBy = userId,
                CreatedDate = DateTime.Now
            };

            await _categoryService.InsertAsync(category); 
            return category;
        }
    }
}
