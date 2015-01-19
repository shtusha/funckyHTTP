using System.ComponentModel.DataAnnotations;

namespace FunckyApp.Models
{
    public class PostBindingModel
    {
        [Required]
        public string Message { get; set; }

        public string InReplyTo { get; set; }
    }
}