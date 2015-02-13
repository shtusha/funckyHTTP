namespace FunckyApp.Models
{

    public class PostViewModel : MessageViewModel
    {
        public MessageViewModel[] Replies { get; set; }

        public Link[] Links { get; set; }
    }

    public class MessageViewModel
    {
        InflatedTextViewModel Message { get; set; }
        public TextFragmentViewModel[] Fragments { get; set; }
        public string Author { get; set; }
        public string CreatedOn { get; set; }
    }

    public class TextFragmentViewModel
    {
        public string InflatedText { get; set; }
        public string OriginalText { get; set; }
        public bool IsInflated { get; set; }
    }

    public class PostsViewModel
    {
        public PostHeaderViewModel[] Posts { get; set; }
        public Link[] Links { get; set; }
    }


    public class PostHeaderViewModel
    {
        public InflatedTextViewModel Title { get; set; }
        public string Author { get; set; }
        public string CreatedOn { get; set; }

        public Link[] Links { get; set; }
    }

    public class InflatedTextViewModel
    {
        public string Original { get; set; }

        public string Inflated { get; set; }

        public int InflationRate { get; set; }
    }
}