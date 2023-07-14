using Microsoft.EntityFrameworkCore;
using MoviesCRUD.Data;
using MoviesCRUD.Repository.Implementation;
using MoviesCRUD.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using MoviesCRUD.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Dbcontext
builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MovieDbContext>();

//Dependency Injections
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IRole, Role>();
builder.Services.AddScoped<ICommentRepository,CommentRepository >();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

//For Mapping Identity Razor pages
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var role = scope.ServiceProvider.GetRequiredService<IRole>();
    await role.Initialize();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();