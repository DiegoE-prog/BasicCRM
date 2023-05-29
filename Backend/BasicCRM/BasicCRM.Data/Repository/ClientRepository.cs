using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

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

        public async Task<List<ClientWithAddress>> GetAllClientsWithAddress()
        {
            var dataList = new List<ClientWithAddress>();

            using (var connection = (SqlConnection) _connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_GetAllClientsWithAddress]";

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    var data = PopulateClientFromReader(reader);

                    dataList.Add(data);
                }

                connection.Close();
            }

            return dataList;
        }

        private ClientWithAddress PopulateClientFromReader(SqlDataReader reader)
        {
            var client = new ClientWithAddress();

            client.ClientID = (Guid)reader["ClientID"];
            client.FirstName = Convert.ToString(reader["FirstName"]);
            client.LastName = Convert.ToString(reader["LastName"]);
            client.DateOfBirth = (DateTime)(reader["DateOfBirth"]);
            client.Email = Convert.ToString(reader["Email"]);
            client.PhoneNumber = Convert.ToString(reader["PhoneNumber"]);

            if (!reader.IsDBNull(6))
            {
                client.AddressID = (Guid)reader["AddressID"];
                client.AddressLine = Convert.ToString(reader["AddressLine"]);
                client.AddressDetails = Convert.ToString(reader["AddressDetails"]);
                client.City = Convert.ToString(reader["City"]);
                client.State = Convert.ToString(reader["State"]);
                client.ZipCode = Convert.ToInt32(reader["ZipCode"]);
                client.Country = Convert.ToString(reader["Country"]);
            }

            return client;
        }
    }
}
