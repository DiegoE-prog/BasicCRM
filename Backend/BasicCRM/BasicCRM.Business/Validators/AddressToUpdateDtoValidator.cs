using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository;
using BasicCRM.Data.Repository.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Validators
{
    public class AddressToUpdateDtoValidator : AbstractValidator<AddressToUpdateDto>
    {
        private readonly IRepository<Address> _repository;
        public AddressToUpdateDtoValidator(IRepository<Address> repository)  
        {
            RuleFor(p => p.AddressID)
                .NotEmpty().WithMessage("{PropertyName} is required");

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

            RuleFor(p => p)
                .MustAsync(AddressExists)
                .WithMessage("There is no address with that ID");

            _repository = repository;
        }

        private async Task<bool> AddressExists(AddressToUpdateDto address, CancellationToken cancellationToken)
        {
            var addressFromDb = await _repository.GetByIdAsync(address.AddressID);

            return !addressFromDb.AddressID.Equals(Guid.Empty);
        }
    }
}
