using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Products.Query
{
    public class GetProductQuery : IRequest<IEnumerable<ProductInfo>>
    {
    }
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, IEnumerable<ProductInfo>>
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly IClaimedService _claimedService;

        public GetProductQueryHandler(ICrudService<ProductInfo> productInfo, IClaimedService claimedService)
        {
            _productInfo = productInfo;
            _claimedService = claimedService;
        }
        public async Task<IEnumerable<ProductInfo>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var storeId = await _claimedService.GetStoreIdAsync();

            if (storeId == 0)
            {
                return Enumerable.Empty<ProductInfo>();
            }

            var productInfos = await _productInfo.GetAllAsync(x=>x.StoreInfoId== storeId);
            return productInfos;
        }
    }
}
