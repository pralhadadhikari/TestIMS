using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Query.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryInfo>>
    {
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly IClaimedService _claimedService;

        public GetCategoriesQueryHandler(ICrudService<CategoryInfo> categoryInfo, IClaimedService claimedService)
        {
            _categoryInfo = categoryInfo;
            _claimedService = claimedService;
        }
        public async Task<IEnumerable<CategoryInfo>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var storeId = await _claimedService.GetStoreIdAsync();
            if (storeId == 0)
            {
                return Enumerable.Empty<CategoryInfo>();
            }

            var categories = await _categoryInfo.GetAllAsync(x=>x.StoreInfoId==storeId);
            return categories;
        }
    }
}
