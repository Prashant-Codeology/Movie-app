using Microsoft.Data.SqlClient;
using MoviesCRUD.Models;
using MoviesCRUD.Repository.Interfaces;
using System.Configuration;
using System.Data;

namespace MoviesCRUD.Repository.SPImplementation
{
    public class SPMovieRepository : IMovieRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
        public SPMovieRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }


        public async Task AddMovie(Movie movie)
        {
           // string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPAddMovie", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", movie.MovieId);
                    command.Parameters.AddWithValue("@MovieName", movie.MovieName);
                    command.Parameters.AddWithValue("@MovieDescription", movie.MovieDescription);
                    command.Parameters.AddWithValue("@MovieGenre", movie.MovieGenre);
                    command.Parameters.AddWithValue("@ImagePath", movie.ImagePath);
                    command.Parameters.AddWithValue("@AverageRating", movie.AverageRating);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // throw new NotImplementedException();
        }

        public async Task DeleteMovie(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPDeleteMovie", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            List<Movie> movies = new List<Movie>();

           // string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPGetAllMovies", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Movie movie = new Movie
                            {
                                MovieId = (Guid)reader["MovieId"],
                                MovieName = reader["MovieName"].ToString(),
                                MovieDescription = reader["MovieDescription"].ToString(),
                                MovieGenre = reader["MovieGenre"].ToString(),
                                ImagePath = reader["ImagePath"].ToString(),
                                AverageRating = Convert.ToDecimal(reader["AverageRating"])
                            };
                            movies.Add(movie);
                        }
                    }
                }
            }

            return movies;
        }

        public async Task<decimal> GetAverageRating(Guid id)
        {
            decimal averageRating = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPGetAverageRating", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MovieId", id);
                    connection.Open();

                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        averageRating = Convert.ToDecimal(result);
                    }
                }
            }
            return averageRating;
        }
        public async Task UpdateMovie(Movie movie)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPUpdateMovie", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", movie.MovieId);
                    command.Parameters.AddWithValue("@MovieName", movie.MovieName);
                    command.Parameters.AddWithValue("@MovieDescription", movie.MovieDescription);
                    command.Parameters.AddWithValue("@MovieGenre", movie.MovieGenre);
                    command.Parameters.AddWithValue("@ImagePath", movie.ImagePath);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<Movie> GetMovieById(Guid id)
        {
            Movie movie = new Movie();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetMovieById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            movie = new Movie
                            {
                                MovieId = (Guid)reader["MovieId"],
                                MovieName = reader["MovieName"].ToString(),
                                MovieDescription = reader["MovieDescription"].ToString(),
                                MovieGenre = reader["MovieGenre"].ToString(),
                                ImagePath = reader["ImagePath"].ToString(),
                                AverageRating = Convert.ToDecimal(reader["AverageRating"])
                            };
                        }
                    }
                }
            }
            return movie;
        }
    }
}



