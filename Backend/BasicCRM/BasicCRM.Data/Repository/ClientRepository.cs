using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace BasicCRM.Data.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DbConnection _connection;
        private DatabaseSettings _dbSettings;

        public ClientRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
            _connection = new DbConnection(_dbSettings.DefaultConnection!);
        }

        public async Task<Guid> CreateAsync(Client data)
        {
            //await ExecuteClientUpsertStoredProcedureAsync("[dbo].[usp_CreateClient]", data);
            DataTable dt = new();
            var clientID = Guid.Empty;

            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_CreateClient]";

                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", data.DateOfBirth);
                command.Parameters.AddWithValue("@Email", data.Email);
                command.Parameters.AddWithValue("@PhoneNumber", data.PhoneNumber);
                command.Parameters.Add("@NewClientID", SqlDbType.UniqueIdentifier);
                command.Parameters["@NewClientID"].Direction = ParameterDirection.ReturnValue;

                if (!data.AddressID.Equals(Guid.Empty))
                    command.Parameters.AddWithValue("@AddressID", data.AddressID);

                connection.Open();
                SqlDataAdapter adapter = new(command);
                //var clientID2 = await command.ExecuteScalarAsync();
                adapter.Fill(dt);
                var clientID2 =  command.Parameters["@NewClientID"].Value;
                connection.Close();

                if (dt.Rows.Count > 0)
                    clientID = (Guid)dt.Rows[0]["NewClientID"];
                

                return clientID;
            }
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
            DataTable dt = new();
            List<Client> clients = new();

            using(var connection = (SqlConnection) _connection.CreateConnection())
            using(var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[usp_GetAllClientsWithAddress]";

                connection.Open();
                SqlDataAdapter da = new(cmd);
                da.Fill(dt);
                connection.Close();
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                clients.Add(MapClientFromDataTable(dt, i));
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
                if (!data.AddressID.Equals(Guid.Empty))
                    command.Parameters.AddWithValue("@AddressID", data.AddressID);

                command.CommandText = storedProcedureName;

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        private Client MapClientFromDataTable(DataTable dt, int i)
        {
            var client = new Client()
            {
                ClientID = (Guid)dt.Rows[i]["ClientID"],
                FirstName = dt.Rows[i]["FirstName"].ToString(),
                LastName = dt.Rows[i]["LastName"].ToString(),
                Email = dt.Rows[i]["Email"].ToString(),
                PhoneNumber = dt.Rows[i]["PhoneNumber"].ToString(),
                DateOfBirth = (DateTime)dt.Rows[i]["DateOfBirth"]
            };

            if (!(dt.Rows[i].IsNull("AddressID")))
            {
                client.AddressID = (Guid)dt.Rows[i]["AddressID"];

                client.Address = new Address()
                {
                    AddressID = (Guid)dt.Rows[i]["AddressID"],
                    AddressLine = dt.Rows[i]["AddressLine"].ToString(),
                    AddressDetails = dt.Rows[i]["AddressDetails"].ToString(),
                    City = dt.Rows[i]["City"].ToString(),
                    State = dt.Rows[i]["State"].ToString(),
                    Country = dt.Rows[i]["Country"].ToString(),
                    ZipCode = Convert.ToInt32(dt.Rows[i]["ZipCode"])
                };
            }

            return client;
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
                client.AddressID = (Guid)reader["AddressID"];

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
