using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;

namespace MoviesCRUD.Repository.Interfaces
{
    public interface ICommentRepository
    {
        Task AddComment(Comment comment);
        Task <IEnumerable<CommentVM>> GetComments(Guid MovieId);
        Task DeleteComment(Guid id);
        Task<Comment> GetCommentById(Guid CommentId);
    }
}
