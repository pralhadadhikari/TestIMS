using IMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.IRepository
{
    public interface ICrudService<TEntity> where TEntity : BaseEntity
    {
        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);
        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        TEntity Get(int? id);
        TEntity Get(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(int? id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsyncIncludingProperties(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        int Delete(TEntity entity);
        Task<TEntity> QueryAsync(string commandText, object param = null, CommandType commandType = CommandType.Text);
        Task<IEnumerable<object>> QueryListAsync(string commandText, object param = null, CommandType commandType = CommandType.Text);
    }
}
