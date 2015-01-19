using FunckyApp.DataAccess;
using FunckyApp.DataAccess.Post;
using FunckyApp.DataAccess.User;

namespace FunckyApp
{
    public partial class Startup
    {
        private static void RegisterRepositories()
        {
            RepositoryFactory.Register<IPostRepository>(new DataAccess.Transient.PostRepository());
            RepositoryFactory.Register<IUserRepository>(new DataAccess.Transient.UserRepository());

            var hashSalt = PasswordUtils.GenerateHashSalt();

            RepositoryFactory.GetRepository<IUserRepository>().SaveAsync(new UserEntity
            {
                HashSalt = hashSalt,
                PasswordHash = "1234567".HashPassword(hashSalt),
                UserName = "FunckyUser",
            });
        }
    }
}