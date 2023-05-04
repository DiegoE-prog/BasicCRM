using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BasicCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {

        private readonly IRepository<Address> _repository;
        public AddressController(IRepository<Address> addressRepository)
        {
            _repository = addressRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            var adddreses = await _repository.GetAllAsync();
            return Ok(adddreses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(Guid id)
        {
            var address = await _repository.GetByIdAsync(id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAddress(Address address)
        {
            await _repository.CreateAsync(address);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAddress(Address address)
        {
            await _repository.UpdateAsync(address);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(Guid id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }

    }
}
