using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.IRepository
{
    public interface IGenericRepository
    {
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, DynamicParameters parameters);
    }
}
