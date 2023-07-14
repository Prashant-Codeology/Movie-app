using Microsoft.EntityFrameworkCore;
using MoviesCRUD.Data;
using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;
using MoviesCRUD.Repository.Interfaces;
using System.Drawing;

namespace MoviesCRUD.Repository.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MovieDbContext _context;

        public CommentRepository(MovieDbContext context)
        {
            _context = context;
        }
        public async Task AddComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(Guid id)
        {
            var cmt = await _context.Comments.FindAsync(id);
            if (cmt != null)
            {
                _context.Comments.Remove(cmt);
                await Save();
            }
        }
        public Task<Comment> GetCommentById(Guid MovieId)
        {
            
            
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommentVM>> GetComments(Guid MovieId)
        {
            return await _context.Comments.Where(d => d.MovieId == MovieId)
               .Select(d => new CommentVM
               {
                   CommentId = d.CommentId,
                   Text = d.Text,
                   UserName = d.User.UserName,
                   CreatedAt = d.CreatedAt
               })
               .ToListAsync();
        }
        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
