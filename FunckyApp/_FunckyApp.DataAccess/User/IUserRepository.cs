using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess.User
{
    public interface IUserRepository : IRepository<UserEntity, string>
    {
    }
}
