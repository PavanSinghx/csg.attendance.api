using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSG.Attendance.Api.Repositories
{
    public interface IRepository<TEntityType>
    {
        Task RemoveAsync(TEntityType data);
        Task RemoveRangeAsync(IEnumerable<TEntityType> dataList);
        Task AddAsync(TEntityType data);
        Task AddRangeAsync(IEnumerable<TEntityType> dataList);
        Task Update(TEntityType data);
        Task UpdateRangeAsync(IEnumerable<TEntityType> dataList);
        Task<bool> ExistsAsync(Expression<Func<TEntityType, bool>> predicate);
        Task<TEntityType> FirstOrDefaultAsync(Expression<Func<TEntityType, bool>> predicate);
        Task<List<TEntityType>> GetAllAsync(Expression<Func<TEntityType, bool>> predicate);
    }
}