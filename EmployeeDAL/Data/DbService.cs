using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAL.Data
{
    public sealed class DbService
    {
        private static readonly Lazy<DbService> _instance = new Lazy<DbService>(() => new DbService());
        private readonly string _connectionString;

        private DbService()
        {
            _connectionString = "Server=(LocalDb)\\MSSQLLocalDb;Database=Practical22;Trusted_Connection=True;";
        }

        public static DbService Instance => _instance.Value;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}