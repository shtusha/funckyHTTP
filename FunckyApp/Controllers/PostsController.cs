using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using FunckyApp.Core;
using FunckyApp.DataAccess;
using FunckyApp.DataAccess.Post;
using FunckyApp.Models;

namespace FunckyApp.Controllers
{
    [RoutePrefix("api/posts")]
    public class PostsController : ApiControllerBase
    {
        private static readonly IPostRepository PostRepository = RepositoryFactory.GetRepository<IPostRepository>();

        private static readonly TranslationEngine TranslationEngine =
            new TranslationEngine(TranslationEngine.InflationaryEnglishDictionary);

        // GET api/posts
        [HttpGet]
        [Route("", Name = "posts.list")]
        public async Task<IHttpActionResult> List()
        {

            var response = await PostRepository.FindAllAsync();

            if (!response.Success) { return GetErrorResult(response); }
            
            
            return Ok(new PostsViewModel
            {
                Posts = response.Entities.Select(a => 
                    new PostHeaderViewModel
                {
                    Author = a.Author,
                    CreatedOn = a.CreatedOn.ToString("MMMM dd, yyyy"),
                    
                    Title = InflatedTextViewModelFromTransaltion(
                        new Translation(a.Message.Select(b => new Fragment(b.OriginalText, b.InflatedText)))),
                    Links = new [] { new Link("details", Url.Route("posts.get", new {id = a.Id}) ) }
                }).ToArray(),
                Links = new[]
                {
                    new Link("new", Url.Route("posts.create", new{})),
                    new Link("preview", Url.Route("posts.preview", new{}))
                }
            });
        }

        // GET api/posts/5
        [HttpGet]
        [Route("{id}", Name = "posts.get")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var response = await PostRepository.GetAsync(id);

            if (!response.Success) { return GetErrorResult(response); }

            if (!response.Found)
            {
                ModelState.AddModelError("post.notFound", "Post Not Found");
                return NotFound();
            }

            return Ok(PostViewModelFromEntity(response.Entity));
        }

        // POST api/posts
        [Authorize]
        [HttpPost]
        [Route("", Name = "posts.create")]
        public async Task<IHttpActionResult> Create(PostBindingModel post)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var entity = CommentEntityFromBindingModel<PostEntity>(post);

            var response = await PostRepository.SaveAsync(entity);

            if (response.Success)
            {
                entity.Id = response.Key;
                return CreatedAtRoute("posts.get", new {id = response.Key }, PostViewModelFromEntity(entity));
            }
            return GetErrorResult(response);
        }

        [HttpPost]
        [Route("preview", Name = "posts.preview")]
        public async Task<IHttpActionResult> Preview(PostBindingModel post)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var translation = TranslationEngine.Translate(post.Message, post.InflationRate);

            return Ok(InflatedTextViewModelFromTransaltion(translation));
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/reply", Name = "posts.reply")]
        public async Task<IHttpActionResult> Reply(PostBindingModel comment, string id)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var response = await PostRepository.AddCommentAsync(id, CommentEntityFromBindingModel<CommentEntity>(comment));

            return response.Success ? Ok() : GetErrorResult(response);

        }

        

        #region Mappers

        private PostViewModel PostViewModelFromEntity(PostEntity entity)
        {
            var postViewModel = CommentViewModelFromEntity<PostViewModel>(entity);

            postViewModel.Replies = (entity.Replies ?? Enumerable.Empty<CommentEntity>())
                .Select(CommentViewModelFromEntity<MessageViewModel>)
                .ToArray();

            postViewModel.Links = new[]
            {
                new Link("self", Url.Route("posts.get", new {id = entity.Id})),
                new Link("reply", Url.Route("posts.reply", new {id = entity.Id})),
                new Link("preview", Url.Route("posts.preview", new{}))
            };

            return postViewModel;
        }


        private T CommentEntityFromBindingModel<T>(PostBindingModel post) where T : CommentEntity, new()
        {
            var translation = TranslationEngine.Translate(post.Message, post.InflationRate);
            return new T
            {
                Author = UserName,
                CreatedOn = DateTime.Now,
                InflationRate = translation.InflationRate,
                Message = translation.Fragments.Select(a => new InflatedTextFragmentEntity
                {
                    OriginalText = a.OriginalText,
                    InflatedText = a.IsInflated ? a.InflatedText : null
                }).ToArray(),
            };
        }

        private T CommentViewModelFromEntity<T>(CommentEntity entity) where T : MessageViewModel, new()
        {
            return new T
            {
                Author = entity.Author,
                CreatedOn = entity.CreatedOn.ToString("MMMM dd, yyyy"),
                Fragments = entity.Message.Select(a => new TextFragmentViewModel
                {
                    OriginalText = a.OriginalText,
                    InflatedText = a.InflatedText,
                    IsInflated = a.InflatedText != null
                }).ToArray()
            };
        }

        private InflatedTextViewModel InflatedTextViewModelFromTransaltion(Translation translation)
        {
            if (translation == null) { return null; }
            return new InflatedTextViewModel
            {
                Inflated = translation.InflatedPhrase,
                InflationRate = translation.InflationRate,
                Original = translation.OriginalPhrase
            };
        }


        #endregion
    }
}