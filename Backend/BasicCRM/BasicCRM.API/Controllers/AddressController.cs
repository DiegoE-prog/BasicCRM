using BasicCRM.Business.Dtos.AddressDto;
using BasicCRM.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAddressDto>>> GetAll()
        {
            var adddreses = await _addressService.GetAllAddressAsync();
            return Ok(adddreses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAddressDto>> GetById(Guid id)
        {
            var address = await _addressService.GetAddressAsync(id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAddress(AddressToCreateDto address)
        {
            await _addressService.CreateAddressAsync(address);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAddress(AddressToUpdateDto address)
        {
            await _addressService.UpdateAddressAsync(address);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(Guid id)
        {
            await _addressService.DeleteAddressAsync(id);
            return Ok();
        }

    }
}
