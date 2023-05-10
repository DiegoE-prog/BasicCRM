using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Business.Dtos.ClientDto;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using FluentValidation;

namespace BasicCRM.Business.Validators
{
    public class ClientToCreateDtoValidator : AbstractValidator<ClientToCreateDto>
    {
        private readonly IRepository<Address> _repository;
        public ClientToCreateDtoValidator(IRepository<Address> repository) 
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(25).WithMessage("Length needs to be lower that 25");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(25).WithMessage("Length needs to be lower that 25");

            RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Must(DateIsValid).WithMessage("You have to enter a valid date");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(40).WithMessage("Length needs to be lower that 40")
                .Matches("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$").WithMessage("{PropertyName} needs to have a valid format");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .Length(10, 15).WithMessage("Length needs to be lower that 15 and higher than 10");

            RuleFor(p => p.AddressID)
                .MustAsync(AddressExists).WithMessage("There is not an address with that ID");

            _repository = repository;
        }

        private async Task<bool> AddressExists(Guid id, CancellationToken cancellationToken)
        {
            if (id.Equals(Guid.Empty)) return true;

            var addressFromDb = await _repository.GetByIdAsync(id);

            return !addressFromDb.AddressID.Equals(Guid.Empty);
        }

        private bool DateIsValid(DateTime date)
        {
            return date < DateTime.Now;
        }
    }
}
