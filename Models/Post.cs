using System.ComponentModel.DataAnnotations;

namespace Blog_April.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
