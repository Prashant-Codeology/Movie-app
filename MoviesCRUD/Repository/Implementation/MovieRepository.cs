using Microsoft.EntityFrameworkCore;
using MoviesCRUD.Data;
using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;
using MoviesCRUD.Repository.Interfaces;
using System.Data;

namespace MoviesCRUD.Repository.Implementation
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return movies;
            //throw new NotImplementedException();
        }
        public async Task<Movie> GetMovieById(Guid id)
        {
            return await _context.Movies.FindAsync(id);
            //throw new NotImplementedException();
        }
        public async Task AddMovie(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await Save();
            // throw new NotImplementedException();
        }
        public async Task UpdateMovie(Movie movie)
        {
            var data = await _context.Movies.FindAsync(movie.MovieId);
            if (data != null)
            {
                data.MovieName = movie.MovieName;
                data.MovieDescription = movie.MovieDescription;
                data.MovieGenre = movie.MovieGenre;
                _context.Update(data);
                await Save();
            }
            // throw new NotImplementedException();
        }
        public async Task DeleteMovie(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if(movie != null)
            {
                _context.Movies.Remove(movie);
                await Save();
            }
        }
        public async Task<decimal> GetAverageRating(Guid id)
        {
            decimal averageRating = await _context.Movies
                .Where(r => r.MovieId == id)
                .Select(r => r.AverageRating)
                .FirstOrDefaultAsync();

            return averageRating;
        }
       


        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
