﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesCRUD.Models
{
    public class Movie
    {
        [Key]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string MovieDescription { get; set; } = string.Empty;
        public string MovieGenre { get; set; }
        public string ImagePath { get; set; }
        [DefaultValue(0)]
        public decimal AverageRating { get; set; }
    }
}
