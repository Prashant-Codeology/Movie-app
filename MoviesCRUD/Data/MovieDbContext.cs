using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using MoviesCRUD.Models;
using MoviesCRUD.Models.ViewModel;

namespace MoviesCRUD.Data
{
    public class MovieDbContext : IdentityDbContext
    {
        public MovieDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Rating { get; set; }

    }
}
