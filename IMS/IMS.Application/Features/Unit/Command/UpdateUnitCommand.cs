using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
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

namespace IMS.Application.Features.Unit.Command
{
    public class UpdateUnitCommand:IRequest<UnitInfo>
    {
        public int Id { get; set; }
        [Required]
        public string UnitName { get; set; }       
        public bool IsActive { get; set; }
    }

    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, UnitInfo>
    {
        private readonly ICrudService<UnitInfo> _UnitInfo;
        private readonly IClaimedService _claimedService;

        public UpdateUnitCommandHandler(ICrudService<UnitInfo> UnitInfo, IClaimedService claimedService)
        {
            _UnitInfo = UnitInfo;
            _claimedService = claimedService;
        }
        public async Task<UnitInfo> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            var Unit = await _UnitInfo.GetAsync(request.Id);
            if (Unit == null)
                return null;

            Unit.UnitName=request.UnitName;
            Unit.IsActive = request.IsActive;
            Unit.ModifiedDate = DateTime.Now;
            Unit.ModifiedBy = _claimedService.UserId;
            await _UnitInfo.UpdateAsync(Unit);

            return Unit;
        }
    }
}
