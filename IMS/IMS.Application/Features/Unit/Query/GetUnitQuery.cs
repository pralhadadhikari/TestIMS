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
    public class GetUnitQuery:IRequest<IEnumerable<UnitInfo>>
    {
    }
    public class GetUnitQueryHandler : IRequestHandler<GetUnitQuery, IEnumerable<UnitInfo>>
    {
        private readonly ICrudService<UnitInfo> _unitInfo;

        public GetUnitQueryHandler(ICrudService<UnitInfo> UnitInfo)
        {
            _unitInfo = UnitInfo;
        }
        public async Task<IEnumerable<UnitInfo>> Handle(GetUnitQuery request, CancellationToken cancellationToken)
        {
            var UnitInfos= await _unitInfo.GetAllAsync();
            return UnitInfos;
        }
    }
}
