using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Categories.Command.DeleteCategories
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICrudService<CategoryInfo> _categoryService;
        public DeleteCategoryCommandHandler(
           ICrudService<CategoryInfo> categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {

            var category = await _categoryService.GetAsync(request.Id);

            if (category == null)
                return false;

            _categoryService.Delete(category);

            return true;


        }
    }
}
