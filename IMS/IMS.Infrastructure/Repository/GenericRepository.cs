using Dapper;
using IMS.Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IDbConnection _dbConnection;

        public GenericRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, DynamicParameters parameters)
        {
            var result = await _dbConnection.QueryAsync<T>(
                storedProcedureName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

    }

}
