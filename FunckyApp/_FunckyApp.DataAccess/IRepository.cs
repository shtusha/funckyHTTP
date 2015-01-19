using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess
{

    public interface IRepository { }

    public interface IRepository<TEntity, TKey> : IRepository where TEntity: class
    {
        Task<GetEntityResponse<TEntity>> GetAsync(TKey key);
        Task<SaveEntityResponse<TKey>> SaveAsync(TEntity entity);
    }
}
