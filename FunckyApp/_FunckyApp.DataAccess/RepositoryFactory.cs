using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess
{
    public static class RepositoryFactory
    {
        private static readonly ConcurrentDictionary<Type, IRepository> _repositories = new ConcurrentDictionary<Type, IRepository>();

        public static void Register<T>(T repository) where T : IRepository
        {
            if(!_repositories.TryAdd(typeof(T), repository))
            {
                throw new ArgumentException(string.Format("Repository for type {0} is already registered", typeof (T)));
            };
        }

        public static T GetRepository<T>() where T : IRepository
        {
            IRepository repo;
            if (_repositories.TryGetValue(typeof (T), out repo))
            {
                return (T) repo;
            }
            throw new ArgumentException(string.Format("No repositories found for type {0}", typeof(T)));
        }
    }
}
