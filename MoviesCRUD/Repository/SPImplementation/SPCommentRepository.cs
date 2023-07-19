using Microsoft.Data.SqlClient;
using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;
using MoviesCRUD.Repository.Interfaces;
using System.ComponentModel.Design;
using System.Data;

namespace MoviesCRUD.Repository.SPImplementation
{
    public class SPCommentRepository : ICommentRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
        public SPCommentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task AddComment(Comment comment)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("AddComment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CommentId", comment.CommentId);
                    command.Parameters.AddWithValue("@Text", comment.Text);
                    command.Parameters.AddWithValue("@UserId", comment.UserId);
                    command.Parameters.AddWithValue("@MovieId", comment.MovieId);
                    command.Parameters.AddWithValue("@CreatedAt", comment.CreatedAt);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task DeleteComment(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPDeleteComment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentId", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public Task<Comment> GetCommentById(Guid CommentId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommentVM>> GetComments(Guid MovieId)
        {
            List<CommentVM> comments = new List<CommentVM>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SPGetCommentsByMovieId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MovieId", MovieId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CommentVM comment = new CommentVM
                            {
                                CommentId = reader.GetGuid(reader.GetOrdinal("CommentId")),
                                Text = reader.GetString(reader.GetOrdinal("Text")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };

                            comments.Add(comment);
                        }
                    }
                }
            }

            return comments;
        }
    }
}
