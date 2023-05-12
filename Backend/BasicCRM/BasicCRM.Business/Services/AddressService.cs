using AutoMapper;
using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Business.Exceptions;
using BasicCRM.Business.Services.Interfaces;
using BasicCRM.Business.Validators;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
namespace BasicCRM.Business.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAddressAsync(AddressToCreateDto addressToCreate)
        {
            var validator = new AddressToCreateDtoValidator();
            var validationResult = validator.Validate(addressToCreate);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid address", validationResult);

            var address = _mapper.Map<Address>(addressToCreate);

            return await _repository.CreateAsync(address);
        }

        public async Task UpdateAddressAsync(AddressToUpdateDto addressToUpdate)
        {
            var validator = new AddressToUpdateDtoValidator(_repository);
            var validationResult = await validator.ValidateAsync(addressToUpdate);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid address", validationResult);

            var address = _mapper.Map<Address>(addressToUpdate);

            await _repository.UpdateAsync(address);
        }

        public async Task DeleteAddressAsync(Guid id)
        {
            var address = new Address();
            address = await _repository.GetByIdAsync(id);

            if (address.AddressID.Equals(Guid.Empty))
                throw new NotFoundException("There is no address with that ID");

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetAddressDto>> GetAllAddressAsync()
        {
            var addresses = new List<GetAddressDto>();
            addresses = _mapper.Map<List<GetAddressDto>>(await _repository.GetAllAsync());

            if(addresses.Count is 0)
                throw new NotFoundException("There are not addresses register");

            return addresses;
        }

        public async Task<GetAddressDto> GetAddressAsync(Guid id)
        {
            var address = new GetAddressDto();
            address = _mapper.Map<GetAddressDto>(await _repository.GetByIdAsync(id));

            if (address.AddressID.Equals(Guid.Empty))
                throw new NotFoundException("There is not an address with that ID");
            
            return address;
        }
    }
}
