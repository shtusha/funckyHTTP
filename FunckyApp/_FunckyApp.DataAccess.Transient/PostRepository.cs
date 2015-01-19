using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FunckyApp.DataAccess.Post;

namespace FunckyApp.DataAccess.Transient
{
    public class PostRepository: IPostRepository
    {
        private readonly ConcurrentDictionary<string, PostEntity> _posts = 
            new ConcurrentDictionary<string, PostEntity>(StringComparer.CurrentCultureIgnoreCase); 
        public async Task<GetEntityResponse<PostEntity>> GetAsync(string key)
        {
            PostEntity entity;

            return new GetEntityResponse<PostEntity>()
            {
                Found = _posts.TryGetValue(key, out entity),
                Entity = entity,
                Success = true
            };
        }

        public async Task<SaveEntityResponse<string>> SaveAsync(PostEntity entity)
        {
            entity.Id = entity.Id ?? Guid.NewGuid().ToString("N");

            //last write wins
            _posts.AddOrUpdate(entity.Id, a => entity, (a, b) => entity);
            return new SaveEntityResponse<string>
            {
                Success = true,
                Key = entity.Id
            };
        }

        public async Task<GetEntitiesResponse<PostEntity>> FindAllAsync()
        {
            return new GetEntitiesResponse<PostEntity>
            {
                Success = true,
                Entities = _posts.Values
            };
        }

        public async Task<RepositoryOperationResponseBase> AddCommentAsync(string key, CommentEntity comment)
        {
            PostEntity entity;

            if (_posts.TryGetValue(key, out entity))
            {
                entity.Replies.Add(comment);
                return new RepositoryOperationResponseBase { Success = true };
            }
            return new RepositoryOperationResponseBase { Success = false };
        }
    }
}