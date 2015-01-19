using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FunckyApp.DataAccess;
using FunckyApp.DataAccess.User;

namespace FunckyApp.DataAccess.Transient
{
    public class UserRepository :IUserRepository
    {
        private readonly ConcurrentDictionary<string, UserEntity> _users = 
            new ConcurrentDictionary<string, UserEntity>(StringComparer.CurrentCultureIgnoreCase);


        public async Task<GetEntityResponse<UserEntity>> GetAsync(string key)
        {
            UserEntity entity;

            return new GetEntityResponse<UserEntity>()
            {
                Success = true,
                Found = _users.TryGetValue(key, out entity),
                Entity = entity
            };
        }

        public async Task<SaveEntityResponse<string>> SaveAsync(UserEntity entity)
        {
            var response = new SaveEntityResponse<string> {Key = entity.UserName};
            if ((entity.UserName ?? entity.PasswordHash ?? entity.HashSalt) == null)
            {
                response.Errors.Add("Missing Required Info");
                return response;
            }
            
            //last write wins
            _users.AddOrUpdate(entity.UserName, a => entity, (a, b) => entity);
            response.Success = true;
            return response;
        }
    }
}