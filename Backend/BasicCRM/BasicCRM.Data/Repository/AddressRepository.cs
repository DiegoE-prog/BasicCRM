using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BasicCRM.Data.Repository
{
    public class AddressRepository : IRepository<Address>
    {
        private readonly DbConnection _connection;
        private DatabaseSettings _dbSettings;

        public AddressRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
            _connection = new DbConnection(_dbSettings.DefaultConnection!);
        }

        public async Task CreateAsync(Address data)
        {
            await ExecuteAddressUpsertStoredProcedureAsync("[dbo].[usp_CreateAddress]", data);
        }

        public async Task UpdateAsync(Address data)
        {
            await ExecuteAddressUpsertStoredProcedureAsync("[dbo].[usp_UpdateAddress]", data);
        }

        public async Task DeleteAsync(Guid id)
        {
            using(var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("AddressID", id);

                command.CommandText = "[dbo].[usp_DeleteAddress]";

                connection.Open();
                
                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            var addresses = new List<Address>();

            using (var connection = (SqlConnection) _connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_GetAllAddresses]";

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    var address = PopulateAddressFromReader(reader);

                    addresses.Add(address);
                }

                connection.Close();
            }

            return addresses;
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            var address = new Address();

            using(var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AddressID", id);
                command.CommandText = "[dbo].[usp_GetClientWithAddressById]";

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    address = PopulateAddressFromReader(reader);
                }

                connection.Close();
            }

            return address;
        }

        private async Task ExecuteAddressUpsertStoredProcedureAsync(string storedProcedureName, Address data)
        {
            using(var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if(storedProcedureName == "[dbo].[usp_UpdateAddress]")
                {
                    command.Parameters.AddWithValue("@AddressID", data.AddressID);
                }

                command.Parameters.AddWithValue("@AddressLine", data.AddressLine);
                command.Parameters.AddWithValue("@AddressDetails", data.AddressDetails);
                command.Parameters.AddWithValue("@City", data.City);
                command.Parameters.AddWithValue("@State", data.State);
                command.Parameters.AddWithValue("@ZipCode", data.ZipCode);
                command.Parameters.AddWithValue("@Country", data.Country);

                command.CommandText = storedProcedureName;

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        private Address PopulateAddressFromReader(SqlDataReader reader)
        {
            var address = new Address();

            address.AddressID = (Guid)(reader["AddressID"]);
            address.AddressLine = Convert.ToString(reader["AddressLine"]);
            address.AddressDetails = Convert.ToString(reader["AddressDetails"]);
            address.City = Convert.ToString(reader["City"]);
            address.State = Convert.ToString(reader["State"]);
            address.ZipCode = Convert.ToInt32(reader["ZipCode"]);
            address.Country = Convert.ToString(reader["Country"]);

            return address;
        }


    }
}
