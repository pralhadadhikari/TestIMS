using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Units.Query
{
    public class GetUnitByIdQuery:IRequest<UnitInfo>
    {
        public int Id { get; set; }
    }

    public class GetUnitByIdQueryHandler : IRequestHandler<GetUnitByIdQuery, UnitInfo>
    {
        private readonly ICrudService<UnitInfo> _unitInfo;

        public GetUnitByIdQueryHandler(ICrudService<UnitInfo> UnitInfo)
        {
            _unitInfo = UnitInfo;
        }
        public async Task<UnitInfo> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitInfo.GetAsync(request.Id);
            return data;
        }
    }
}
