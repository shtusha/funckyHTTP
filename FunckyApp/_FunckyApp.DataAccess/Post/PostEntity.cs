using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunckyApp.DataAccess.Post
{
    public class PostEntity : CommentEntity
    {
        public PostEntity()
        {
            Replies = new List<CommentEntity>();
        }

        public string Id { get; set; }
        public List<CommentEntity> Replies { get; set; }

    }

    public class InflatedTextFragmentEntity
    {
        public string OriginalText { get; set; }
        public string InflatedText { get; set; }
    }

    public class CommentEntity
    {
        public InflatedTextFragmentEntity[] Message { get; set; }
        public string Author { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
