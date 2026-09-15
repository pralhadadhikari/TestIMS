using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Services;
using IMS.Models.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Products.Command
{
    public class ProductCommand:IRequest<ProductInfo>
    {
        public int Id { get; set; }
        [Required]
        public int CategoryInfoId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public int UnitInfoId { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<ProductCommand, ProductInfo>
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly IClaimedService _claimedService;
        private readonly IFileService _fileService;

        public UpdateProductCommandHandler(ICrudService<ProductInfo> productInfo, IClaimedService claimedService,
            IFileService fileService)
        {
            _productInfo = productInfo;
            _claimedService = claimedService;
            _fileService = fileService;
        }
        public async Task<ProductInfo> Handle(ProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productInfo.GetAsync(request.Id);
            if (product == null)
                return null;

            product.CategoryInfoId=request.CategoryInfoId;
            product.ProductName=request.ProductName;
            product.ProductDescription=request.ProductDescription;
            product.UnitInfoId=request.UnitInfoId;            
            product.IsActive = request.IsActive;
            product.ModifiedDate = DateTime.Now;
            product.ModifiedBy = _claimedService.UserId;
            string oldImage = null;
            if (request.ImageFile != null &&
               request.ImageFile.Length > 0)
            {
                oldImage = product.ImageUrl;

                product.ImageUrl =
                    await _fileService.SaveFileAsync(
                        request.ImageFile,"ProductImage",cancellationToken);
            }



            await _productInfo.UpdateAsync(product);
            if (!string.IsNullOrEmpty(oldImage))
            {
                await _fileService.DeleteFileAsync(oldImage);
            }
            return product;
        }
    }
}
