using System.ComponentModel.DataAnnotations;

namespace MoviesCRUD.Models.ViewModel
{
    public class MovieUpdateVM
    {

        [Key]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; } = string.Empty;
        public string MovieGenre { get; set; }
    }
}
