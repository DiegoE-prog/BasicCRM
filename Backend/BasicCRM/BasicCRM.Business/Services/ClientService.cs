using AutoMapper;
using BasicCRM.Business.Dtos.ClientDto;
using BasicCRM.Business.Exceptions;
using BasicCRM.Business.Services.Interfaces;
using BasicCRM.Business.Validators;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;

namespace BasicCRM.Business.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IMapper _mapper;   
        public ClientService(IRepository<Client> clientRepository,
            IRepository<Address> addressRepository,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _addressRepository = addressRepository;

            _mapper = mapper;
        }

        public async Task CreateClientAsync(ClientToCreateDto clientToCreate)
        {
            var validator = new ClientToCreateDtoValidator(_addressRepository);
            var validationResults = await validator.ValidateAsync(clientToCreate);

            if (validationResults.Errors.Any())
                throw new BadRequestException("Invalid client", validationResults);

            var address = _mapper.Map<Client>(clientToCreate);

            await _clientRepository.CreateAsync(address);
        }

        public async Task UpdateClientAsync(ClientToUpdateDto clientToUpdate)
        {
            var validator = new ClientToUpdateDtoValidator(_addressRepository, _clientRepository);
            var validationResults = await validator.ValidateAsync(clientToUpdate);

            if (validationResults.Errors.Any())
                throw new BadRequestException("Invalid client", validationResults);

            var client = _mapper.Map<Client>(clientToUpdate);

            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client.ClientID.Equals(Guid.Empty))
                throw new NotFoundException("There is not a client with that ID");

            await _clientRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetClientDto>> GetAllClientsAsync()
        {
            var clients = _mapper.Map<List<GetClientDto>>(await _clientRepository.GetAllAsync());

            if (clients.Count is 0)
                throw new NotFoundException("There are not clients registered");

            return clients;
        }

        public async Task<GetClientDto> GetClientAsync(Guid id)
        {
            var client = _mapper.Map<GetClientDto>(await _clientRepository.GetByIdAsync(id));

            if (client.ClientID.Equals(Guid.Empty))
                throw new NotFoundException("There is not a client with that ID");

            return client;
        }
    }
}
