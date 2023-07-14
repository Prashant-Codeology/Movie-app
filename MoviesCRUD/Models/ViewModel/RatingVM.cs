using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesCRUD.Models.ViewModel
{
    public class RatingVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Foreign key to the User table
        public Guid MovieId { get; set; }  // Foreign key to the Movie table
        [Range(1, 5)]
        public int Value { get; set; }  // Rating value between 1 and 5
    }
}
