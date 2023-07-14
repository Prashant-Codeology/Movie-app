namespace MoviesCRUD.Models.ViewModel
{
    public class AddCommentVM
    {
        public string? Text { get; set; }
        public string? UserId { get; set; }
        public Guid MovieId { get; set; }
        public string? UserName { get; set; } // Add UserName property
    }
}
