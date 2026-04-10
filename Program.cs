using Microsoft.EntityFrameworkCore;
using MVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieContext") ?? "Data Source=Movies.db"));

var app = builder.Build();

// Seed the database on startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MovieContext>();
    context.Database.EnsureCreated();

    if (!context.Movie.Any())
    {
        context.Movie.AddRange(
            new Movie { Title = "The Matrix", ReleaseDate = new DateTime(1999, 3, 31), Genre = "Science Fiction", Price = 9.99m },
            new Movie { Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16), Genre = "Action", Price = 12.50m },
            new Movie { Title = "Back to the Future", ReleaseDate = new DateTime(1985, 7, 3), Genre = "Adventure", Price = 8.99m },
            new Movie { Title = "Interstellar", ReleaseDate = new DateTime(2014, 11, 7), Genre = "Drama", Price = 11.00m }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
