using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess
{
    public class RepositoryOperationResponseBase
    {
        public RepositoryOperationResponseBase ()
        {
            Errors = new List<string>();
        }
        public bool Success { get; set; }

        //Can be a more formal error object. Good ennough for our needs
        public List<string> Errors { get; private set; }
    }

    public class GetEntityResponse<TEntity> : RepositoryOperationResponseBase
    {
        public bool Found { get; set; }
        public TEntity Entity { get; set; }
    }

    public class SaveEntityResponse<TKey> : RepositoryOperationResponseBase
    {
        public TKey Key { get; set; }
    }

    public class GetEntitiesResponse<TEntity> : RepositoryOperationResponseBase
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }
}
