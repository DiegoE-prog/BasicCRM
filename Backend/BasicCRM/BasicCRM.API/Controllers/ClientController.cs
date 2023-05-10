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

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetClientDto>>> GetAll()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetClientDto>> GetById(Guid id)
        {
            var client = await _clientService.GetClientAsync(id);
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClientToCreateDto client)
        {
            await _clientService.CreateClientAsync(client);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(ClientToUpdateDto client)
        {
            await _clientService.UpdateClientAsync(client);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _clientService.DeleteClientAsync(id);
            return Ok();
        }
    }
}
