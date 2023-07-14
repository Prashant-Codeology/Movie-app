using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesCRUD.Models
{
    public class Rating
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }  // Foreign key to the User table

        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }  // Foreign key to the Movie table

        [Range(1, 5)]
        public int Value { get; set; }  // Rating value between 1 and 5

        // Navigation properties
        public virtual IdentityUser User { get; set; }  // Reference to the User entity
        public virtual Movie Movie { get; set; }  // Reference to the Movie entity
    }
}
