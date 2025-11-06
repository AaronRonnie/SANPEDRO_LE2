using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace BlogDataLibrary.SQL
{
    public class SqlDataAccess
    {
        private readonly string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> LoadData<T>(string sql, object parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(sql, parameters);
            }
        }

        public void SaveData(string sql, object parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, parameters);
            }
        }
    }
}
