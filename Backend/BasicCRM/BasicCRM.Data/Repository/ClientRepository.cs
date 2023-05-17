using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace BasicCRM.Data.Repository
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(IOptions<DatabaseSettings> dbSettings) : base(dbSettings) 
        {
            SpCreate = "[dbo].[usp_CreateClient]";
            SpUpdate = "[dbo].[usp_UpdateClient]";
            SpDelete = "[dbo].[usp_DeleteClient]";
            SpGetAll = "[dbo].[usp_GetAllClients]";
            SpGetById = "[dbo].[usp_GetClientById]";
        }
    }
}
