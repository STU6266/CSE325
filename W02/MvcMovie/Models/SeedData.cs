
using MvcMovie.Data;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context =
            serviceProvider.GetRequiredService<MvcMovieContext>();

        var favorites = new (string SearchText, Movie Movie)[]
        {
            (
                "Lord of the Rings",
                new Movie
                {
                    Title = "The Lord of the Rings: The Fellowship of the Ring",
                    ReleaseDate = new DateTime(2001, 12, 19),
                    Genre = "Fantasy",
                    Price = 9.99M,
                    Rating = "PG-13"
                }
            ),
            (
                "Hobbit",
                new Movie
                {
                    Title = "The Hobbit: An Unexpected Journey",
                    ReleaseDate = new DateTime(2012, 12, 14),
                    Genre = "Fantasy",
                    Price = 8.99M,
                    Rating = "PG-13"
                }
            ),
            (
                "Zombieland",
                new Movie
                {
                    Title = "Zombieland",
                    ReleaseDate = new DateTime(2009, 10, 2),
                    Genre = "Comedy",
                    Price = 7.99M,
                    Rating = "R"
                }
            )
        };

        var existingTitles = context.Movie
            .Select(m => m.Title)
            .ToList();

        foreach (var (searchText, movie) in favorites)
        {
            bool alreadyExists = existingTitles.Any(
                title => title?.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase) == true);

            if (!alreadyExists)
            {
                context.Movie.Add(movie);
            }
        }

        context.SaveChanges();
    }
}
