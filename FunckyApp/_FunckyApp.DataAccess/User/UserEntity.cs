using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess.User
{
    public class UserEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string HashSalt { get; set; }
    }
}
