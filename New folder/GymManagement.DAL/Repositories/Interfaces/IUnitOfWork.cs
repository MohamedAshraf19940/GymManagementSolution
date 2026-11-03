using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        //public IGenericRepository<Plan> PlanRepository { get; set; }
        //public IGenericRepository<Member> MemberRepository { get; set; }


        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        public ISessionRepository SessionRepository { get;  }
    }
}
