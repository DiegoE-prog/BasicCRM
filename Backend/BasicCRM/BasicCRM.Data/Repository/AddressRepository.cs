using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace BasicCRM.Data.Repository 
{ 
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(IOptions<DatabaseSettings> dbSettings) :base(dbSettings)
        {
            SpCreate = "[dbo].[usp_CreateAddress]";
            SpUpdate = "[dbo].[usp_UpdateAddress]";
            SpDelete = "[dbo].[usp_DeleteAddress]";
            SpGetAll = "[dbo].[usp_GetAllAddresses]";
            SpGetById = "[dbo].[usp_GetAddressById]";
        }
    }
}
