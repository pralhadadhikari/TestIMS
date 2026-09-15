using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Features.Unit.Command
{
    public class CreateUnitValidator : AbstractValidator<CreateUnitCommand>
    {
        public CreateUnitValidator()
        {
            //RuleFor(x => x.ProductName)
            //    .NotEmpty()
            //    .WithMessage("Category name is required.")
            //    .MaximumLength(100)
            //    .WithMessage("Category name cannot exceed 100 characters.");

            //RuleFor(x => x.CategoryDescription)
            //    .NotEmpty()
            //    .WithMessage("Category description is required.")
            //    .MaximumLength(500)
            //    .WithMessage("Category description cannot exceed 500 characters.");

            //RuleFor(x => x.StoreInfoId)
            //    .GreaterThan(0)
            //    .WithMessage("Store is required.");
        }
    }
}
