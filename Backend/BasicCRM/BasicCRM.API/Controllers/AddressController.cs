using BasicCRM.API.Models;
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
        private Response response = new();
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAddressDto>>> GetAll()
        {
            response.Content = await _addressService.GetAllAddressAsync();
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetAddressById")]
        public async Task<ActionResult<GetAddressDto>> GetById(Guid id)
        {
            response.Content = await _addressService.GetAddressAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAddress(AddressToCreateDto address)
        {
            response.Content = await _addressService.CreateAddressAsync(address);
            response.Message = "Address created successfully";
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAddress(AddressToUpdateDto address)
        {
            await _addressService.UpdateAddressAsync(address);
            response.Message = "Address updated successfully";
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(Guid id)
        {
            await _addressService.DeleteAddressAsync(id);
            response.Message = "Address deleted successfully";
            return Ok(response);
        }

    }
}
