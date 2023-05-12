using BasicCRM.Business.Dtos.ClientDto;

namespace BasicCRM.Business.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<GetClientDto>> GetAllClientsAsync();
        Task<GetClientDto> GetClientAsync(Guid id);
        Task<Guid> CreateClientAsync(ClientToCreateDto clientCreate);
        Task UpdateClientAsync(ClientToUpdateDto clientToUpdate);
        Task DeleteClientAsync(Guid id);
    }
}
