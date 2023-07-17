using MoviesCRUD.Models;

namespace MoviesCRUD.Repository.Interfaces
{
    public interface IRatingRepository
    {
        Task AddRating(Rating rate);
        Task<decimal> GetRatingValue(Guid MovieId, string UserId);
        Task<bool> HasRated(Guid MovieId, string UserId);
        Task<int> RatingCount(Guid MovieId);
    }
}
