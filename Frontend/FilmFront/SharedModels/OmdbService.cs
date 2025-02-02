using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmFront.Components.SharedModels;

namespace FilmFront.Components.SharedModels
{
    public class OmdbService
    {
        private readonly HttpClient _httpClient;
        public OmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<OmdbFilmDetail>> GetFilm(string title)
        {
            var response = await _httpClient.GetAsync($"api/Omdb/search/{title}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<OmdbFilmDetail>>();
            }

            return null;
        }
    }
}