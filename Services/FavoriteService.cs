using Microsoft.JSInterop;
using MovieTime.Components.Models;
using System.Text.Json;

namespace MovieTime.Services
{
    public class FavoriteService(IJSRuntime jSRuntime)
    {
        private readonly string _localStorageKey = "favorite_movies";


        /// <summary>
        /// Returns a list of movies stored in local storage as favorites.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Movie>> GetFavorites()
        {
            List<Movie> movies = [];

            try
            {
                var json = await jSRuntime.InvokeAsync<string>("localStorage.getItem", _localStorageKey);
                movies = JsonSerializer.Deserialize<List<Movie>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return movies;
        }

        /// <summary>
        /// Saves a list of favorite movies to local storage.
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        public async Task SaveFavorites(List<Movie> movies)
        {
            try
            {
                var json = JsonSerializer.Serialize(movies);
                await jSRuntime.InvokeVoidAsync("localStorage.setItem", _localStorageKey, json);
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }   
        }

        /// <summary>
        /// Adds the specified movie to the list of favorites if it is not already present.
        /// </summary>
        /// <param name="movie">The movie to add to the favorites list. Cannot be null. If a movie with the same identifier already exists
        /// in the favorites, it will not be added again.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        public async Task AddFavorite(Movie movie)
        {
            var currentMovies = await GetFavorites();

            if(currentMovies.All(m => m.Id != movie.Id))
            {
                currentMovies.Add(movie);
                await SaveFavorites(currentMovies);
            }
        }

        /// <summary>
        /// Removes the specified movie from the user's list of favorite movies.
        /// </summary>
        /// <param name="movie">The movie to remove from the favorites list.</param>
        /// <returns>A task that represents the asynchronous remove operation.</returns>
        public async Task RemoveFavorite(Movie movie)
        {
            var currentMovies = await GetFavorites();
            currentMovies = currentMovies.Where(m => m.Id != movie.Id).ToList();

            await SaveFavorites(currentMovies);
        }

        /// <summary>
        /// Determines whether the movie with the specified identifier is marked as a favorite.
        /// </summary>
        /// <param name="id">The unique identifier of the movie to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// movie is a favorite; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> IsFavorite(int id)
        {
            var currentMovies = await GetFavorites();
            bool isFavorite = currentMovies.Any(m => m.Id == id);

            return isFavorite;
        }
    }
}
