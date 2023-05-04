using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicCRM.Data.Repository.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Guid id);
        Task CreateAsync (TEntity data);
        Task UpdateAsync (TEntity data);
        Task DeleteAsync (Guid id);
    }
}
