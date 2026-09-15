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
    public class GetProductByIdQuery:IRequest<ProductInfo>
    {
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductInfo>
    {
        private readonly ICrudService<ProductInfo> _productInfo;

        public GetProductByIdQueryHandler(ICrudService<ProductInfo> productInfo)
        {
            _productInfo = productInfo;
        }
        public async Task<ProductInfo> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _productInfo.GetAsync(request.Id);
            return data;
        }
    }
}
