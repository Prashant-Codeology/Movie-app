namespace MoviesCRUD.Models.ViewModel
{
    public class MovieDetailsVM
    {
        public Movie? Movie { get; set; }
        public CommentsVM? Comments { get; set; }
        public RatingVM? Rating { get; set;}
    }
}
