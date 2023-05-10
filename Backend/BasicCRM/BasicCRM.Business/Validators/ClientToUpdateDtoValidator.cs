using BasicCRM.Business.Dtos.ClientDto;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository;
using BasicCRM.Data.Repository.Interfaces;
using FluentValidation;

namespace BasicCRM.Business.Validators
{
    public class ClientToUpdateDtoValidator : AbstractValidator<ClientToUpdateDto>
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Client> _clientRepository;
        public ClientToUpdateDtoValidator(IRepository<Address> addressRepository,
                                          IRepository<Client> clientRepository)
        {
            RuleFor(p => p.ClientID)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MustAsync(ClientExists).WithMessage("There is not a client with that ID");

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

            _addressRepository = addressRepository;
            _clientRepository = clientRepository;
        }

        private async Task<bool> AddressExists(Guid id, CancellationToken arg2)
        {
            if(id.Equals(Guid.Empty)) return true; 

            var addressFromDb = await _addressRepository.GetByIdAsync(id);

            return !addressFromDb.AddressID.Equals(Guid.Empty);
        }

        private async Task<bool> ClientExists(Guid id, CancellationToken arg2)
        {
            var clientFromDb = await _clientRepository.GetByIdAsync(id);

            return !clientFromDb.ClientID.Equals(Guid.Empty);
        }

        private bool DateIsValid(DateTime date)
        {
            return date < DateTime.Now;
        }
    }
}
