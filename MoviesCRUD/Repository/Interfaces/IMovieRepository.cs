using MoviesCRUD.Models;

namespace MoviesCRUD.Repository.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMovies();
        Task<Movie> GetMovieById(Guid id);
        Task AddMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(Guid id);
        Task<decimal> GetAverageRating(Guid id);

    }
}
