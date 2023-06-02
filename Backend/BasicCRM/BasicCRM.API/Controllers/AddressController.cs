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
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<Response>> GetAll()
        {
            Response response = new();
            response.Content = await _addressService.GetAllAddressAsync();
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetAddressById")]
        public async Task<ActionResult<Response>> GetById(Guid id)
        {
            Response response = new();
            response.Content = await _addressService.GetAddressAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateAddress(AddressToCreateDto address)
        {
            Response response = new();
            response.Content = await _addressService.CreateAddressAsync(address);
            response.Message = "Address created successfully";
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateAddress(AddressToUpdateDto address)
        {
            Response response = new();
            await _addressService.UpdateAddressAsync(address);
            response.Message = "Address updated successfully";
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteAddress(Guid id)
        {
            Response response = new();
            await _addressService.DeleteAddressAsync(id);
            response.Message = "Address deleted successfully";
            return Ok(response);
        }

    }
}
