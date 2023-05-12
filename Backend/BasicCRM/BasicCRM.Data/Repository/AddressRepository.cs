using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BasicCRM.Data.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DbConnection _connection;
        private DatabaseSettings _dbSettings;

        public AddressRepository(IOptions<DatabaseSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
            _connection = new DbConnection(_dbSettings.DefaultConnection!);
        }

        public async Task<Guid> CreateAsync(Address data)
        {
            DataTable dt = new();
            var addressID = Guid.Empty;

            using (var connection = (SqlConnection) _connection.CreateConnection()) 
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_CreateAddress]";

                command.Parameters.AddWithValue("@AddressID", data.AddressID);
                command.Parameters.AddWithValue("@AddressLine", data.AddressLine);
                command.Parameters.AddWithValue("@AddressDetails", data.AddressDetails);
                command.Parameters.AddWithValue("@City", data.City);
                command.Parameters.AddWithValue("@State", data.State);
                command.Parameters.AddWithValue("@ZipCode", data.ZipCode);
                command.Parameters.AddWithValue("@Country", data.Country);

                command.Parameters.Add("@NewAddressID", SqlDbType.UniqueIdentifier);
                command.Parameters["@NewAddressID"].Direction = ParameterDirection.ReturnValue;

                connection.Open();

                //await command.ExecuteNonQueryAsync();

                SqlDataAdapter adapter = new(command);

                adapter.Fill(dt);

                connection.Close();

                if (dt.Rows.Count > 0)
                    addressID = (Guid) dt.Rows[0]["NewAddressID"];
                

                return addressID;
            }
        }

        public async Task UpdateAsync(Address data)
        {
            //return await ExecuteAddressUpsertStoredProcedureAsync("[dbo].[usp_UpdateAddress]", data);
            using(var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[usp_UpdateAddress]";

                command.Parameters.AddWithValue("@AddressID", data.AddressID);
                command.Parameters.AddWithValue("@AddressLine", data.AddressLine);
                command.Parameters.AddWithValue("@AddressDetails", data.AddressDetails);
                command.Parameters.AddWithValue("@City", data.City);
                command.Parameters.AddWithValue("@State", data.State);
                command.Parameters.AddWithValue("@ZipCode", data.ZipCode);
                command.Parameters.AddWithValue("@Country", data.Country);

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
                command.CommandText = "[dbo].[usp_GetAddressById]";

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

        private async Task<Guid> ExecuteAddressUpsertStoredProcedureAsync(string storedProcedureName, Address data)
        {
            var addressID = Guid.Empty;

            using(var connection = (SqlConnection)_connection.CreateConnection())
            {
                DataTable dt = new DataTable();

                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedureName;

                command.Parameters.AddWithValue("@AddressLine", data.AddressLine);
                command.Parameters.AddWithValue("@AddressDetails", data.AddressDetails);
                command.Parameters.AddWithValue("@City", data.City);
                command.Parameters.AddWithValue("@State", data.State);
                command.Parameters.AddWithValue("@ZipCode", data.ZipCode);
                command.Parameters.AddWithValue("@Country", data.Country);

                connection.Open();

                if (storedProcedureName == "[dbo].[usp_UpdateAddress]")
                {
                    
                    addressID = data.AddressID;

                    
                }

                if (storedProcedureName == "[dbo].[usp_CreateAddress]")
                {
                    command.Parameters.Add("@NewAddressID", SqlDbType.UniqueIdentifier);
                    command.Parameters["@NewAddressID"].Direction = ParameterDirection.ReturnValue;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        addressID = (Guid)dt.Rows[0]["NewAddressID"];
                    }
                }

                connection.Close();
            }
            return addressID;
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
