using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BasicCRM.Data.Repository
{
    public class ClientRepository : IRepository<Client>
    {
        private readonly DbConnection _connection;
        private DatabaseSettings _dbSettings;

        public ClientRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
            _connection = new DbConnection(_dbSettings.DefaultConnection!);
        }

        public async Task CreateAsync(Client data)
        {
            await ExecuteClientUpsertStoredProcedureAsync("[dbo].[usp_CreateClient]", data);
        }

        public async Task UpdateAsync(Client data)
        {
            await ExecuteClientUpsertStoredProcedureAsync("[dbo].[usp_UpdateClient]", data);
        }

        public async Task DeleteAsync(Guid id)
        {
            using(var connection = (SqlConnection) _connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientID", id);
                command.CommandText = "[dbo].[usp_DeleteClient]";

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            var clients = new List<Client>();

            using(var connection = (SqlConnection) _connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_GetAllClientsWithAddress]";

                connection.Open();
                
                SqlDataReader reader = command.ExecuteReader();

                while(await reader.ReadAsync())
                {
                    var client = PopulateClientFromReader(reader);

                    clients.Add(client);
                }

                connection.Close();
            }

            return clients;
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            var client = new Client();

            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientID", id);
                command.CommandText = "[dbo].[usp_GetClientWithAddressById]";

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    client = PopulateClientFromReader(reader);
                }

                connection.Close();
            }
            return client;
        }

        private async Task ExecuteClientUpsertStoredProcedureAsync(string storedProcedureName, Client data)
        {
            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if(storedProcedureName == "[dbo].[usp_UpdateClient]")
                {
                    command.Parameters.AddWithValue("@ClientID", data.ClientID);
                }

                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", data.DateOfBirth);
                command.Parameters.AddWithValue("@Email", data.Email);
                command.Parameters.AddWithValue("@PhoneNumber", data.PhoneNumber);
                if (data.AddressID is not null)
                    command.Parameters.AddWithValue("@AddressID", data.AddressID);

                command.CommandText = storedProcedureName;

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        private Client PopulateClientFromReader(SqlDataReader reader)
        {
            var client = new Client();

            client.ClientID = (Guid)reader["ClientID"];
            client.FirstName = Convert.ToString(reader["FirstName"]);
            client.LastName = Convert.ToString(reader["LastName"]);
            client.DateOfBirth = (DateTime)(reader["DateOfBirth"]);
            client.Email = Convert.ToString(reader["Email"]);
            client.PhoneNumber = Convert.ToString(reader["PhoneNumber"]);

            if (!reader.IsDBNull(6))
            {
                var address = new Address();

                address.AddressID = (Guid)reader["AddressID"];
                address.AddressLine = Convert.ToString(reader["AddressLine"]);
                address.AddressDetails = Convert.ToString(reader["AddressDetails"]);
                address.City = Convert.ToString(reader["City"]);
                address.State = Convert.ToString(reader["State"]);
                address.ZipCode = Convert.ToInt32(reader["ZipCode"]);
                address.Country = Convert.ToString(reader["Country"]);

                client.Address = address;
            }

            return client;
        }
    }
}
