using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Services;
using IMS.Models.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Products.Command
{
    public class CreateProductCommand:IRequest<ProductInfo>
    {
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

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductInfo>
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly IClaimedService _claimedService;
        private readonly IFileService _fileService;

        public CreateProductCommandHandler(ICrudService<ProductInfo> productInfo, IClaimedService claimedService,
            IFileService fileService)
        {
            _productInfo = productInfo;
            _claimedService = claimedService;
            _fileService = fileService;
        }
        public async Task<ProductInfo> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var storeid = await _claimedService.GetStoreIdAsync();
            ProductInfo product = new();
            product.CategoryInfoId = request.CategoryInfoId;
            product.ProductName = request.ProductName;
            product.ProductDescription = request.ProductDescription;
            product.UnitInfoId = request.UnitInfoId;
            product.StoreInfoId = storeid;
            product.IsActive = request.IsActive;
            product.CreatedDate = DateTime.Now;
            product.CreatedBy = _claimedService.UserId;


            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                product.ImageUrl = await _fileService.SaveFileAsync(
                    request.ImageFile,"ProductImage",cancellationToken);
            }


            await _productInfo.InsertAsync(product);

            return product;
        }
    }

}
