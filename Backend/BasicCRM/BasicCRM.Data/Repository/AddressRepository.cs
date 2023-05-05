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
            using(var connection = (SqlConnection) _connection.CreateConnection()) 
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AddressID", data.AddressID);
                command.Parameters.AddWithValue("@AddressLine", data.AddressLine);
                command.Parameters.AddWithValue("@AddressDetails", data.AddressDetails);
                command.Parameters.AddWithValue("@City", data.City);
                command.Parameters.AddWithValue("@State", data.State);
                command.Parameters.AddWithValue("@ZipCode", data.ZipCode);
                command.Parameters.AddWithValue("@Country", data.Country);

                command.CommandText = "[dbo].[usp_CreateAddress]";

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
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
                    Address address = new Address();

                    address.AddressID = (Guid) (reader["AddressID"]);
                    address.AddressLine = Convert.ToString(reader["AddressLine"]);
                    address.AddressDetails = Convert.ToString(reader["AddressDetails"]);
                    address.City = Convert.ToString(reader["City"]);
                    address.State = Convert.ToString(reader["State"]);
                    address.ZipCode = Convert.ToInt32(reader["ZipCode"]);
                    address.Country = Convert.ToString(reader["Country"]);

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
                    address.AddressID = (Guid)(reader["AddressID"]);
                    address.AddressLine = Convert.ToString(reader["AddressLine"]);
                    address.AddressDetails = Convert.ToString(reader["AddressDetails"]);
                    address.City = Convert.ToString(reader["City"]);
                    address.State = Convert.ToString(reader["State"]);
                    address.ZipCode = Convert.ToInt32(reader["ZipCode"]);
                    address.Country = Convert.ToString(reader["Country"]);
                }

                connection.Close();
            }

            return address;
        }

        public async Task UpdateAsync(Address data)
        {
            using(var connection = (SqlConnection) _connection.CreateConnection())
            {
                var command = connection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AddressID", data.AddressID);
                command.Parameters.AddWithValue("@AddressLine", data.AddressLine);
                command.Parameters.AddWithValue("@AddressDetails", data.AddressDetails);
                command.Parameters.AddWithValue("@City", data.City);
                command.Parameters.AddWithValue("@State", data.State);
                command.Parameters.AddWithValue("@ZipCode", data.ZipCode);
                command.Parameters.AddWithValue("@Country", data.Country);

                command.CommandText = "[dbo].[usp_UpdateAddress]";

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }
    }
}
