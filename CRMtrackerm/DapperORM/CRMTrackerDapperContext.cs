using Npgsql;
using System.Data;

namespace CrmTracker.DapperORM
{
    public class CRMtrackerDapperContext
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public CRMtrackerDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
