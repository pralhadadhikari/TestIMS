using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Unit.Command
{
    public class DeleteUnitValidator : AbstractValidator<DeleteUnitCommand>
    {
        public DeleteUnitValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid product ID.");
        }
    }
}
