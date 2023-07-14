using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesCRUD.Models.ViewModel;
using MoviesCRUD.Models;
using MoviesCRUD.Repository.Implementation;
using MoviesCRUD.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MoviesCRUD.Controllers
{
    public class RatingController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        public RatingController(UserManager<IdentityUser> userManager, IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            _userManager = userManager;
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([Bind("MovieId,Value")] RatingVM rate)
        {
            rate.UserId = _userManager.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
            int count = await _ratingRepository.RatingCount(rate.MovieId);
            Rating rt = new()
            {
                Value = rate.Value,
                MovieId = rate.MovieId,
                UserId = rate.UserId,
            };
            await _ratingRepository.AddRating(rt);

            await CalculateAverageRating(count, rt.Value, rt.MovieId);
            var averagerating = await _movieRepository.GetAverageRating(rt.MovieId);
            return Content(averagerating.ToString());
        }

        public async Task CalculateAverageRating(int count, int rate, Guid MovieId)
        {
            var movie = await _movieRepository.GetMovieById(MovieId);

            if (movie != null)
            {
                var newAverage = ((count * (movie.AverageRating)) + rate) / (count + 1);
                movie.AverageRating = newAverage;
                await _movieRepository.UpdateMovie(movie);
            }
        }


    }
}

