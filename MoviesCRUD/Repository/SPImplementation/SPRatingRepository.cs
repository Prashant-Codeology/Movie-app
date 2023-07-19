using Microsoft.Data.SqlClient;
using MoviesCRUD.Models;
using MoviesCRUD.Repository.Interfaces;
using Newtonsoft.Json.Linq;
using System.Data;

namespace MoviesCRUD.Repository.SPImplementation
{
    public class SPRatingRepository : IRatingRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
        public SPRatingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task AddRating(Rating rate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPAddRating", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserId", rate.UserId);
                        command.Parameters.AddWithValue("@MovieId", rate.MovieId);
                        command.Parameters.AddWithValue("@Value", rate.Value);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            
        }

        public async Task<decimal> GetRatingValue(Guid MovieId, string UserId)
        {
            int ratingValue = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPGetRatingValue", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", MovieId);
                    command.Parameters.AddWithValue("@UserId", UserId);

                    connection.Open();

                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        ratingValue = (int)result;
                    }
                }
            }

            return (decimal)ratingValue;

        }

        public async Task<bool> HasRated(Guid MovieId, string UserId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPHasRated", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", MovieId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.Add("@Exists", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    connection.Open();
                    await command.ExecuteNonQueryAsync();

                    bool exists = (bool)command.Parameters["@Exists"].Value;
                    return exists;
                }
            }
        }

        public async Task<int> RatingCount(Guid MovieId)
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPRatingCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MovieId", MovieId);
                    command.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;

                    connection.Open();
                    await command.ExecuteNonQueryAsync();

                    if (command.Parameters["@Count"].Value != DBNull.Value)
                    {
                        count = (int)command.Parameters["@Count"].Value;
                    }
                }
            }

            return count;
        }
    }
}
