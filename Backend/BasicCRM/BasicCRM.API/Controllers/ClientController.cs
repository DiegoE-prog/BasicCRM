using BasicCRM.API.Models;
using BasicCRM.Business.Dtos.ClientDto;
using BasicCRM.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private Response response = new();

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetClientDto>>> GetAll()
        {
            response.Content = await _clientService.GetAllClientsAsync();
            return Ok(response);
        }

        [HttpGet("{id}",Name = "GetClientByID")]
        public async Task<ActionResult<GetClientDto>> GetById(Guid id)
        {
            response.Content = await _clientService.GetClientAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClientToCreateDto client)
        {
            response.Content = await _clientService.CreateClientAsync(client);
            response.Message = "Client created successfully";
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> Update(ClientToUpdateDto client)
        {
            await _clientService.UpdateClientAsync(client);
            response.Message = "Client updated successfully";
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _clientService.DeleteClientAsync(id);
            response.Message = "Client deleted successfully";
            return Ok(response);
        }
    }
}
