using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity :BaseEntity, new()
    { 

        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        void Add(TEntity TEntity  );
        void Update(TEntity TEntity);
        void Delete(TEntity TEntity);

        Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken ct = default);
    }
}
