using IMS.Application.Features.Categories.Command.DeleteCategories;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Services;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Products.Command
{
    public class DeleteProductCommand:IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly IFileService _fileService;

        public DeleteProductCommandHandler(
           ICrudService<ProductInfo> productInfo,IFileService fileService)
        {
            _productInfo = productInfo;
            _fileService = fileService;
        }
        
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productInfo.GetAsync(request.Id);

            if (product == null)
                return false;

            var imageUrl = product.ImageUrl;

            // Delete product from database
            _productInfo.Delete(product);

            // Delete physical image
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                await _fileService.DeleteFileAsync(imageUrl);
            }

            return true;
        }
    }
}
