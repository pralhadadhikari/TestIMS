using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Query.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryInfo>
    {
        private readonly ICrudService<CategoryInfo> _categoryInfo;

        public GetCategoryByIdQueryHandler(ICrudService<CategoryInfo> categoryInfo)
        {
            _categoryInfo = categoryInfo;
        }
        public async Task<CategoryInfo> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)            
                return null;
            

            var category=await _categoryInfo.GetAsync(request.Id);
            return category;
        }
    }
}
