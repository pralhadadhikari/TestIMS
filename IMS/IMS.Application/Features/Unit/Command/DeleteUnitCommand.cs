using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Unit.Command
{
    public class DeleteUnitCommand:IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, bool>
    {
        private readonly ICrudService<UnitInfo> _UnitInfo;

        public DeleteUnitCommandHandler(
           ICrudService<UnitInfo> UnitInfo)
        {
            _UnitInfo = UnitInfo;
        }

        public async Task<bool> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var Unit = await _UnitInfo.GetAsync(request.Id);

            if (Unit == null)
                return false;

            _UnitInfo.Delete(Unit);

            return true;
        }
    }
}
