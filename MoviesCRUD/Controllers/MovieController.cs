using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;
using MoviesCRUD.Repository.Interfaces;
using System.Xml.Linq;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MoviesCRUD.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRatingRepository _ratingRepository;
        private readonly IHostingEnvironment _environment;
        private readonly ICommentRepository _commentRepository;


        public MovieController(IMovieRepository movieRepository, IHostingEnvironment environment,
            ICommentRepository commentRepository, UserManager<IdentityUser> userManager,
            IRatingRepository ratingRepository)
        {
            _movieRepository = movieRepository;
            _environment = environment;
            _commentRepository = commentRepository;
            _userManager = userManager;
            _ratingRepository = ratingRepository;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.GetAllMovies();
            return View(movies);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateVM movie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var path = _environment.WebRootPath;
                    var filePath = "Content/Image/" + movie.MovieId +movie.ImagePath.FileName;
                    var fullPath = Path.Combine(path, filePath);
                    UploadFile(movie.ImagePath, fullPath);
                    var newmovie = new Movie()
                    {
                        MovieId = movie.MovieId,
                        MovieName = movie.MovieName,
                        MovieDescription = movie.MovieDescription,
                        MovieGenre = movie.MovieGenre,
                        ImagePath = filePath
                    };

                     await _movieRepository.AddMovie(newmovie);
                    TempData["SuccessMessages"] = "Movie created sucessfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessages"] = "Model state is invalid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessages"] = ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id)
        {
            var data = await _movieRepository.GetMovieById(id);
            if (data != null)
            {
                var movie = new MovieUpdateVM()
                {
                    MovieId = data.MovieId,
                    MovieName = data.MovieName,
                    MovieDescription = data.MovieDescription,
                    MovieGenre = data.MovieGenre
                };
                return View(movie);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, MovieUpdateVM movie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMovie = await _movieRepository.GetMovieById(id);

                    if (existingMovie == null)
                    {
                        TempData["ErrorMessages"] = "Movie not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    existingMovie.MovieName = movie.MovieName;
                    existingMovie.MovieDescription = movie.MovieDescription;
                    existingMovie.MovieGenre = movie.MovieGenre;

                    await _movieRepository.UpdateMovie(existingMovie);
                    TempData["SuccessMessages"] = " Movie updated sucessfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessages"] = "Model state is invalid";
                    return View(movie);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessages"] = ex.Message;
                return View(movie);
            }
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var movie = await _movieRepository.GetMovieById(id);
            var comments = await _commentRepository.GetComments(id);
            CommentsVM commentsVM = new()
            {
                Comments = (IEnumerable<CommentVM>)comments
            };
            MovieDetailsVM details = new()
            {
                Movie = movie,
                Comments = commentsVM 
            };
            if (User.Identity.IsAuthenticated)
            {
                var userid = _userManager.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
                ViewBag.HasRated = await _ratingRepository.HasRated(movie.MovieId, userid);
                if (ViewBag.HasRated)
                {
                    ViewBag.UserRating = await _ratingRepository.GetRatingValue(movie.MovieId, userid);
                }
            }

            if (details != null)
            {
                return View(details);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _movieRepository.GetMovieById(id);

            if (movie != null)
            {
                return View(movie);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            try
            {
                await _movieRepository.DeleteMovie(id);
                TempData["SuccessMessages"] = "Movie is deleted sucessfully!!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessages"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: UpdateImage/Edit/5
        public async Task<IActionResult> UpdateImage(Guid id)
        {
            var data = await _movieRepository.GetMovieById(id);
            if (data != null)
            {
                var movie = new UploadImageVM()
                {
                    MovieId = data.MovieId,
                    MovieName = data.MovieName,
                    MovieDescription = data.MovieDescription,
                    MovieGenre = data.MovieGenre,
                    ImageURL = data.ImagePath
                };
                return View(movie);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        // POST: UpdateImage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateImage(Guid id, UploadImageVM movie)
        {
            try
            {
                var data = await _movieRepository.GetMovieById(id);


                if (data == null)
                {
                    return NotFound();
                }

                // Delete the old image file
               // DeleteImageFile(data.ImagePath);

                // Save the new image file

                var path = _environment.WebRootPath;
                var filePath = "Content/Image/" + movie.ImagePath.FileName;
                var fullPath = Path.Combine(path, filePath);
                UploadFile(movie.ImagePath, fullPath);

                data.ImagePath = filePath;

              //  await _dbContext.SaveChangesAsync();
                await _movieRepository.UpdateMovie(data);
                TempData["SuccessMessages"] = "Image is changed sucessfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Handle the exception according to your needs
                TempData["ErrorMessage"] = "Failed to update image path.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Helper method to delete the image file
        private void DeleteImageFile(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string filePath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }






        public void UploadFile(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }
    }
}

