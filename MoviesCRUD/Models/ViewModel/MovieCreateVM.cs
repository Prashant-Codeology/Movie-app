using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesCRUD.Models.ViewModel
{
    public class MovieCreateVM
    {
        [Key]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; }=string.Empty;
        public string MovieGenre { get; set; }
        [Display(Name = "Choose File")]
        public IFormFile ImagePath { get; set; }
    }
}
