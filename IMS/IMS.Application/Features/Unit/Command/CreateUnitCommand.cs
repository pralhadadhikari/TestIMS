using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
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

namespace IMS.Application.Features.Unit.Command
{
    public class CreateUnitCommand:IRequest<UnitInfo>
    {
        [Required]
        public string UnitName { get; set; }       
        public bool IsActive { get; set; }
    }

    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, UnitInfo>
    {
        private readonly ICrudService<UnitInfo> _unitInfo;
        private readonly IClaimedService _claimedService;

        public CreateUnitCommandHandler(ICrudService<UnitInfo> UnitInfo, IClaimedService claimedService)
        {
            _unitInfo = UnitInfo;
            _claimedService = claimedService;
        }
        public async Task<UnitInfo> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
            UnitInfo Unit = new();
            Unit.UnitName = request.UnitName;
            Unit.IsActive = request.IsActive;
            Unit.CreatedDate = DateTime.Now;
            Unit.CreatedBy = _claimedService.UserId;
            await _unitInfo.InsertAsync(Unit);

            return Unit;
        }
    }

}
