using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FilmFront.Components.SharedModels;

namespace FilmFront.Components.SharedModels
{
    public class FilmService
    {
        private readonly HttpClient _httpClient;
        public FilmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Film>> GetFilm()
        {
            var response = await _httpClient.GetAsync($"api/Film/Films");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Film>>();
            }

            return null;
        }
    }
}