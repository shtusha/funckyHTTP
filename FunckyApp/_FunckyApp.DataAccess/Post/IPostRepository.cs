using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess.Post
{
    public interface IPostRepository : IRepository<PostEntity, string>
    {
        Task<GetEntitiesResponse<PostEntity>> FindAllAsync();
        Task<RepositoryOperationResponseBase> AddCommentAsync(string key, CommentEntity comment);

    }
}
