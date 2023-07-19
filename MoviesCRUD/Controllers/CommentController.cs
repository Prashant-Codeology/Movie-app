using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesCRUD.Data;
using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;
using MoviesCRUD.Repository.Interfaces;

namespace MoviesCRUD.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICommentRepository _commentRepository;
        public CommentController(UserManager<IdentityUser> userManager, ICommentRepository commentRepository)
        {
            _userManager = userManager;
            _commentRepository = commentRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment([Bind("MovieId,Text")] AddCommentVM comment)
        {
            comment.UserId = _userManager.GetUserId(User) ?? throw new ArgumentNullException(nameof(User));
            Comment cmt = new()
            {
                CommentId = Guid.NewGuid(),
                Text = comment.Text,
                MovieId = comment.MovieId,
                UserId = comment.UserId,
                CreatedAt = DateTime.Now
            };
            await _commentRepository.AddComment(cmt);
            return RedirectToAction("ViewComment", new { MovieId = comment.MovieId });
        }
        [HttpGet]
        public async Task<IActionResult> ViewComment(Guid MovieId)
        {
            var comments = await _commentRepository.GetComments(MovieId);
            CommentsVM cmts = new CommentsVM()
            {
                Comments= comments
            };
            return PartialView("_ViewComment", cmts);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(Guid MovieId, Guid CommentId)
        {
            await _commentRepository.DeleteComment(CommentId);
            return RedirectToAction("ViewComment", new { MovieId = MovieId });
        }
    }
}
