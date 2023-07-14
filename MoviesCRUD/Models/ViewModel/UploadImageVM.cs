using System.ComponentModel.DataAnnotations;

namespace MoviesCRUD.Models.ViewModel
{
    public class UploadImageVM
    {
        [Key]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; } = string.Empty;
        public string MovieGenre { get; set; }
        [Display(Name = "Choose File")]
        [Required]
        public IFormFile ImagePath { get; set; }
        public string ImageURL { get; set; }
    }
}
