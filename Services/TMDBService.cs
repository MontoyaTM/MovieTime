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
                _http.DefaultRequestHeaders.Authorization = new("Bearer", tmdbKey);
            }
        }

        public async Task<MovieListResponse> GetNowPlayingMovies()
        {
            string url = "https://api.themoviedb.org/3/movie/now_playing?region=US&language=en-US";

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

        public async Task<MovieListResponse> GetPopularMovies()
        {
            string url = "https://api.themoviedb.org/3/movie/popular?region=US&language=en-US";

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
        /// Search for movies
        /// </summary>
        /// <param name="query">User supplied query</param>
        /// <returns></returns>
        /// <exception cref="HttpIOException"></exception>
        public async Task<MovieListResponse> SearchMovies(string query)
        {
            string url = $"https://api.themoviedb.org/3/search/movie?query={query}&include_adult=false&language=en-us";

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

        public async Task<MovieDetails> GetMovieById(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}";

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

        public async Task<Video?> GetMovieTrailer(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}/videos?language=en-US";

            MovieVideoResponse videos = await _http.GetFromJsonAsync<MovieVideoResponse>(url, _jsonOptions)
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Could not retrieve movie trailer");

            Video? movieTrailer = videos.Results
                .FirstOrDefault(v => v.Type!.Contains("Trailer", StringComparison.OrdinalIgnoreCase) 
                                && v.Site!.Contains("YouTube", StringComparison.OrdinalIgnoreCase));

            return movieTrailer;
        }

        public async Task<CreditsResponse> GetMovieCredits(int movieId)
        {
            string url = $"https://api.themoviedb.org/3/movie/{movieId}/credits?language=en-US";

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
