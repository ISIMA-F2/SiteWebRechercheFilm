using TrackerDeFavorisApi.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TrackerDeFavorisApi.Services
{
    public class OmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public OmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OmdbApi:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey), "L'API Key OMDB est introuvable.");
        }
        public async Task<List<Film>> SearchFilmsByTitleAsync(string title)
        {
            var response = await _httpClient.GetStringAsync($"https://www.omdbapi.com/?s={title}&apikey={_apiKey}");
            var searchResult = System.Text.Json.JsonSerializer.Deserialize<OmdbSearchResponse>(response);
            var films = new List<Film>();

            if (searchResult?.Search != null)
            {
                foreach (var omdbFilm in searchResult.Search)
                {
                    films.Add(new Film
                    {
                        Title = omdbFilm.Title,
                        Year = 0,
                        Imdb = omdbFilm.ImdbID,
                        Poster = omdbFilm.Poster
                    });
                }
            }

            return films;
        }
        public async Task<Film?> GetFilmByImdbIdAsync(string imdbId)
        {
            var response = await _httpClient.GetStringAsync($"https://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}");
            var filmDetail = System.Text.Json.JsonSerializer.Deserialize<OmdbFilmDetail>(response);
            if (filmDetail != null)
            {
                return new Film
                {
                    Title = filmDetail.Title,
                    Year = Int32.Parse(filmDetail.Year),
                    Imdb = filmDetail.ImdbID,
                    Poster = filmDetail.Poster
                };
            }

            return null;
        }
    }
}
