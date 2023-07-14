using Microsoft.EntityFrameworkCore;
using MoviesCRUD.Data;
using MoviesCRUD.Models;
using MoviesCRUD.Repository.Interfaces;
using System.Linq;

namespace MoviesCRUD.Repository.Implementation
{
    public class RatingRepository : IRatingRepository
    {

        private readonly MovieDbContext _context;

        public RatingRepository(MovieDbContext context)
        {
            _context = context;
        }
        public async Task AddRating(Rating rate)
        {
            await _context.Rating.AddAsync(rate);
            await Save();

           // throw new NotImplementedException();
        }

        public async Task<bool> HasRated(Guid MovieId, string UserId)
        {
            bool exists = await _context.Rating.AnyAsync(r => r.MovieId == MovieId && r.UserId == UserId);
            return exists;

            //throw new NotImplementedException();
        }

        public async Task<int> RatingCount(Guid MovieId)
        {

            return _context.Rating.Where(m => m.MovieId == MovieId).Count();
          //  throw new NotImplementedException();
        }


        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
