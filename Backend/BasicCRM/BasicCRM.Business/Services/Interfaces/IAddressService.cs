using BasicCRM.Business.Dtos.AddressDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Business.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<GetAddressDto>> GetAllAddressAsync();
        Task<GetAddressDto> GetAddressAsync(Guid id);
        Task CreateAddressAsync(AddressToCreateDto addressDto);
        Task UpdateAddressAsync(AddressToUpdateDto addressToUpdate);
        Task DeleteAddressAsync(Guid id);
    }
}
