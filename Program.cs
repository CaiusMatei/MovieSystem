global using Microsoft.EntityFrameworkCore;
global using MovieSystemTMDB.Properties.Data;
using MovieSystemTMDB.Properties.Models;
using System.Text.Json;

namespace MovieSystemTMDB;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var client = new HttpClient();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<MovieTmdbDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();  
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Information from TMDB api by using theirs API key
        app.MapGet("/api/DiscoverTitels/", async (MovieTmdbDbContext context, string GenreName) =>
        {
            var client = new HttpClient();
            var gen = await context.Genres.SingleOrDefaultAsync(g => g.Title == GenreName);
            string TmdbApiKey = "";
            var apiUrl = $"https://api.themoviedb.org/3/discover/movie?api_key={TmdbApiKey}&sort_by=popularity.desc&include_adult=false&include_video=false&with_genres={gen.GenreId}&with_watch_monetization_types=free";
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return Results.Content(content, contentType: "application/json");
        });

        // Information about every person in Database
        app.MapGet("/api/Person/", async (MovieTmdbDbContext context) =>
        {
            var query = from persons in context.Persons
                        select new { persons.FirstName, persons.Email };
            return await query.ToListAsync();
        });


        // Get Genre linked to a person

        app.MapGet("/api/PersonGenre/", async (MovieTmdbDbContext context, string FirstName) =>
        {
            var pGenre = from pg in context.PersonGenres
                         select new
                         {
                             pg.Persons.FirstName,
                             pg.Genres.Title
                         };
            var getTheGenres = pGenre.GroupBy(x => x.FirstName)
                .Select(x => new { FirstName = x.Key, LikedGenres = string.Join(", ", x.Select(y => y.FirstName)) })
                .Where(x => x.FirstName == FirstName).ToListAsync();

            return await getTheGenres;
        });

        // Get movies by person name
        app.MapGet("/api/Person/GetMovies/{name}", async (MovieTmdbDbContext context, string name) =>
        {
            var movies = await context.Movies
                .Where(movie => movie.Person.FirstName == name)
                .Select(movie => new
                {
                    movie.MovieName,
                    movie.MovieLink,
                    movie.MovieId,
                    movie.Person.FirstName
                })
                .ToListAsync();

            return movies;
        });

        // Get genres liked by person
        app.MapGet("/api/Person/GetGenres/{title}", async (MovieTmdbDbContext context, string title) =>
        {
            var personGenres = await context.PersonGenres
                .Where(pg => pg.Persons.FirstName == title)
                .Select(pg => new
                {
                    pg.Persons.FirstName,
                    pg.Genres.Title
                })
                .ToListAsync();

            return personGenres;
        });

        // Get all genres
        app.MapGet("/api/Genre/GetAll", async (MovieTmdbDbContext context) =>
        {
            var genres = await context.Genres.ToListAsync();
            return genres;
        });

        // Get all persons
        app.MapGet("/api/Persons/GetAll", async (MovieTmdbDbContext context) =>
        {
            var persons = await context.Persons.ToListAsync();
            return persons;
        });

        // Get ratings on movies by person name
        app.MapGet("/api/Person/GetRatings/{name}", async (MovieTmdbDbContext context, string name) =>
        {
            var ratings = await context.MovieRatings
                .Where(pr => pr.Person.FirstName == name)
                .Select(pr => new
                {
                    pr.Movie.MovieName,
                    pr.Rating,
                    pr.Person.FirstName
                })
                .ToListAsync();

            return ratings;
        });

        // Add ratings to movies linked to a person
        app.MapPost("/api/Movie/AddRating", async (MovieTmdbDbContext context, string movieName, int rating) =>
        {
            var movie = await context.Movies.FirstOrDefaultAsync(m => m.MovieName == movieName);

            if (movie == null)
            {
                return Results.NotFound();
            }

            var movieRating = new MovieRating
            {
                Rating = rating,
                MovieId = movie.MovieId,
                PersonId = movie.PersonId
            };
            context.MovieRatings.Add(movieRating);
            await context.SaveChangesAsync();

            return Results.Created($"/api/Movie/AddRating", rating);
        });

        // Add genre to a person
        app.MapPost("/api/Person/AddGenre", async (MovieTmdbDbContext context, string personName, int genreId) =>
        {
            var person = await context.Persons.SingleOrDefaultAsync(p => p.FirstName == personName);

            if (person == null)
            {
                return Results.NotFound();
            }

            var personGenre = new PersonGenre
            {
                PersonId = person.PersonId,
                GenreId = genreId
            };
            context.PersonGenres.Add(personGenre);
            await context.SaveChangesAsync();

            return Results.Created($"/api/Person/AddGenre", genreId);
        });

        // Add movie link to a specific person and genre
        app.MapPost("/api/Person/AddMovieLink", async (MovieTmdbDbContext context, string personName, string movieName, string genreTitle, string movieLink) =>
        {
            var person = await context.Persons.SingleOrDefaultAsync(p => p.FirstName == personName);
            if (person == null)
            {
                return Results.NotFound();
            }

            var existingMovie = await context.Movies.FirstOrDefaultAsync(m => m.MovieName == movieName);
            if (existingMovie != null)
            {
                return Results.BadRequest();
            }

            var genre = await context.Genres.FirstOrDefaultAsync(g => g.Title == genreTitle);
            if (genre == null)
            {
                return Results.BadRequest();
            }

            var movie = new Movie
            {
                PersonId = person.PersonId,
                MovieName = movieName,
                GenreId = genre.GenreId,
                MovieLink = movieLink
            };
            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            return Results.Created($"/api/Person/AddMovieLink", movie);
        });

       
        // Get movies by person name including genre
        app.MapGet("/api/Person/GetMoviesGenre/{Titel}", async (MovieTmdbDbContext context, string title) =>
        {
            var movies = await context.Movies
                .Join(context.Persons, movie => movie.PersonId, person => person.PersonId, (movie, person) => new
                {
                    movie.MovieName,
                    movie.MovieLink,
                    movie.Genre.Title,
                    person.FirstName
                })
                .Where(x => x.FirstName == title)
                .ToListAsync();

            return movies;
        });

        app.Run();
    }
}

