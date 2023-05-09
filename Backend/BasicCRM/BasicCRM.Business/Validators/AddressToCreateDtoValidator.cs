using BasicCRM.Business.Dtos.AddressDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Validators
{
    public class AddressToCreateDtoValidator : AbstractValidator<AddressToCreateDto>
    {
        public AddressToCreateDtoValidator()
        {
            RuleFor(p => p.AddressLine)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.City)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.State)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(p => p.ZipCode)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(x => x.ToString().Length == 5).WithMessage("{PropertyName} need to be 5 digits");

            RuleFor(p => p.Country)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }
    }
}
