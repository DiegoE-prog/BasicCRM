using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BasicCRM.Data.Repository
{
    public class BaseRepository<T> : IRepository<T>
    {
        private readonly DbConnection _connection;
        private DatabaseSettings _dbSettings;
        public string SpGetAll { get; set; } = String.Empty;
        public string SpGetById { get; set; } = String.Empty;
        public string SpCreate { get; set; } = String.Empty;
        public string SpUpdate { get; set; } = String.Empty;
        public string SpDelete { get; set; } = String.Empty;

        public BaseRepository(IOptions<DatabaseSettings> dbSettings)
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            _dbSettings = dbSettings.Value;
            if(isDevelopment)
            {
                _connection = new DbConnection(_dbSettings.LocalConnection!);
            }
            else
            {
                _connection = new DbConnection(_dbSettings.AzureConnection!);
            }

        }

        public async Task<Guid> CreateAsync(T data)
        {
            DataTable dt = new();
            var ID = Guid.Empty;

            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpCreate;

                foreach (var prop in typeof(T).GetProperties())
                {
                    var value = prop.GetValue(data);
                    command.Parameters.AddWithValue($"@{prop.Name}", value ?? DBNull.Value);
                }

                command.Parameters.Add("@NewID", SqlDbType.UniqueIdentifier);
                command.Parameters["@NewID"].Direction = ParameterDirection.ReturnValue;

                connection.Open();

                SqlDataAdapter adapter = new(command);

                adapter.Fill(dt);

                connection.Close();

                if (dt.Rows.Count > 0)
                    ID = (Guid)dt.Rows[0]["NewID"];

                return ID;
            }
        }

        public async Task UpdateAsync(T data)
        {
            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpUpdate;

                foreach (var prop in typeof(T).GetProperties())
                {
                    var value = prop.GetValue(data);
                    command.Parameters.AddWithValue($"@{prop.Name}", value ?? DBNull.Value);
                }

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", id);

                command.CommandText = SpDelete;

                connection.Open();

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var dataList = new List<T>();

            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SpGetAll;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    var data = PopulateDataFromReader(reader);

                    dataList.Add(data);
                }

                connection.Close();
            }

            return dataList;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var data = Activator.CreateInstance<T>();

            using (var connection = (SqlConnection)_connection.CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", id);
                command.CommandText = SpGetById;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    data = PopulateDataFromReader(reader);
                }
                connection.Close();
            }

            return data;
        }

        private T PopulateDataFromReader(SqlDataReader reader)
        {
            var entity = Activator.CreateInstance<T>();

            // Map the data from the database to the entity properties
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var prop = typeof(T).GetProperty(reader.GetName(i));

                if (prop != null && prop.CanWrite)
                {
                    var value = reader.GetValue(i);
                    prop.SetValue(entity, value == DBNull.Value ? null : value);
                }
            }

            return entity;
        }
    }
}