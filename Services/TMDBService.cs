using MovieTime.Components.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace MovieTime.Services
{
    public class TMDBService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public TMDBService(HttpClient httpClient, IConfiguration config)
        {
            _http = httpClient;
            string? tmdbKey = config["TMDB_Key"];

            if(!string.IsNullOrWhiteSpace(tmdbKey))
            {
                _http.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                _http.DefaultRequestHeaders.Authorization = new("Bearer", tmdbKey);
            }
            else
            {
                // Deployed to Netlify
                _http.BaseAddress = new Uri(_http.BaseAddress + "tmdb/");
            }
        }

        /// <summary>
        /// Retrieves a list of movies that are currently playing in theaters in the US using The Movie Database (TMDb) API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a
        /// cref="MovieListResponse"/> object with details about the now-playing movies. The list will be empty if no movies are currently playing.</returns>
        /// <exception cref="HttpIOException">Thrown if the HTTP response is invalid or the now-playing movies data cannot be retrieved from the remote
        /// service.</exception>
        public async Task<MovieListResponse> GetNowPlayingMovies()
        {
            string url = "movie/now_playing?region=US&language=en-US";

            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve now-playing movies response.");

            foreach (Movie movie in response.Results)
            {
                if (string.IsNullOrWhiteSpace(movie.PosterPath))
                {
                    movie.PosterPath = "/images/Poster.png";
                }
                else
                {
                    movie.PosterPath = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
                }
            }

            return response;
        }

        /// <summary>
        /// Retrieves a list of popular movies from The Movie Database (TMDb) API.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MovieListResponse object with
        /// details of popular movies. The list will be empty if no popular movies are found.</returns>
        /// <exception cref="HttpIOException">Thrown if the response from the movie database API is invalid or cannot be retrieved.</exception>
        public async Task<MovieListResponse> GetPopularMovies()
        {
            string url = "movie/popular?region=US&language=en-US";

            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to retrieve popular movies response.");

            foreach(Movie movie in response.Results)
            {
                if (string.IsNullOrWhiteSpace(movie.PosterPath))
                {
                    movie.PosterPath = "/images/Poster.png";
                }
                else
                {
                    movie.PosterPath = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
                }
            }

            return response;
        }

        /// <summary>
        /// Searches for movies that match the specified query string using The Movie Database (TMDb) API.
        /// </summary>
        /// <param name="query">The search text to use for finding matching movie titles. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MovieListResponse with the
        /// list of movies matching the search criteria. The list will be empty if no movies are found.</returns>
        /// <exception cref="HttpIOException">Thrown if the search results cannot be loaded due to an invalid or failed HTTP response.</exception>
        public async Task<MovieListResponse> SearchMovies(string query)
        {
            string url = $"search/movie?query={query}&include_adult=false&language=en-us";

            MovieListResponse response = await _http.GetFromJsonAsync<MovieListResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Search results cannot be loaded.");

            foreach (Movie movie in response.Results)
            {
                if (string.IsNullOrWhiteSpace(movie.PosterPath))
                {
                    movie.PosterPath = "/images/Poster.png";
                }
                else
                {
                    movie.PosterPath = $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
                }
            }

            return response;
        }

        /// <summary>
        /// Retrieves detailed information for a movie by its unique identifier.
        /// </summary>
        /// <remarks>The returned MovieDetails object will have fully qualified URLs for poster and
        /// backdrop images. If the movie does not have a poster or backdrop, default image paths are
        /// provided.</remarks>
        /// <param name="movieId">The unique identifier of the movie to retrieve. Must be a valid movie ID as recognized by the external movie
        /// database.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MovieDetails object with
        /// information about the specified movie.</returns>
        /// <exception cref="HttpIOException">Thrown if the movie details cannot be retrieved due to an invalid response from the external service.</exception>
        public async Task<MovieDetails> GetMovieById(int movieId)
        {
            string url = $"movie/{movieId}";

            MovieDetails movie = await _http.GetFromJsonAsync<MovieDetails>(url, _jsonOptions) 
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Could not retrieve movie details");

            movie.PosterPath = string.IsNullOrEmpty(movie.PosterPath) 
                ? "/images/poster.png" 
                : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";

            movie.BackdropPath = string.IsNullOrEmpty(movie.BackdropPath.ToString()) 
                ? "/images/backdrop.jpg" 
                : $"https://image.tmdb.org/t/p/w780{movie.BackdropPath}";

            return movie;
        }

        /// <summary>
        /// Retrieves the first available YouTube trailer video for the specified movie from The Movie Database (TMDb) API.
        /// </summary>
        /// <param name="movieId">The unique identifier of the movie for which to retrieve the trailer.</param>
        /// <returns>A Video object representing the YouTube trailer for the specified movie, or
        /// null if no trailer is found.</returns>
        /// <exception cref="HttpIOException">Thrown when the movie trailer information cannot be retrieved due to an invalid response from the external
        /// service.</exception>
        public async Task<Video?> GetMovieTrailer(int movieId)
        {
            string url = $"movie/{movieId}/videos?language=en-US";

            MovieVideoResponse videos = await _http.GetFromJsonAsync<MovieVideoResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Could not retrieve movie trailer");

            Video? movieTrailer = videos.Results
                .FirstOrDefault(v => v.Type!.Contains("Trailer", StringComparison.OrdinalIgnoreCase) 
                                && v.Site!.Contains("YouTube", StringComparison.OrdinalIgnoreCase));

            return movieTrailer;
        }

        /// <summary>
        /// Retrieves the cast and crew credits for a specified movie from The Movie Database (TMDb).
        /// </summary>
        /// <param name="movieId">The unique identifier of the movie for which to retrieve credits.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a CreditsResponse object with
        /// the cast and crew information for the specified movie.</returns>
        /// <exception cref="HttpIOException">Thrown if the movie credits cannot be retrieved due to an invalid response from the remote server.</exception>
        public async Task<CreditsResponse> GetMovieCredits(int movieId)
        {
            string url = $"movie/{movieId}/credits?language=en-US";

            var credits = await _http.GetFromJsonAsync<CreditsResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Could not retrieve movie credits");

            foreach(var cast in credits.Cast)
            {
                cast.ProfilePath = string.IsNullOrEmpty(cast.ProfilePath)
                    ? "/images/Profile.jpg"
                    : $"https://image.tmdb.org/t/p/w500{cast.ProfilePath}";
            }

            foreach (var crew in credits.Crew)
            {
                crew.ProfilePath = string.IsNullOrEmpty(crew.ProfilePath)
                    ? "/images/Profile.jpg"
                    : $"https://image.tmdb.org/t/p/w500{crew.ProfilePath}";
            }

            return credits;
        }
    }
}
