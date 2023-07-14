using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesCRUD.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string? Text { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
