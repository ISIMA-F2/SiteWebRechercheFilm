using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FilmFront.Components.SharedModels;

namespace FilmFront.Components.SharedModels
{
    public class FavorisService
    {
        private readonly HttpClient _httpClient;
        public FavorisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Favoris>> GetFavoris(string userId)
        {
            var response = await _httpClient.GetAsync($"api/Favoris/List?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Favoris>>();
            }

            throw new Exception("Failed to add favorite");
        }

        public async Task<Favoris> AddFavoris(Favoris favoris)
{
    try
    {
        Console.WriteLine($"Sending favoris data: UserId={favoris.UserId}");

        var response = await _httpClient.PostAsJsonAsync("api/Favoris/add-favorite", favoris);

        Console.WriteLine($"Response status: {response.StatusCode}");
        var rawContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Raw response content: {rawContent}");

        if (response.IsSuccessStatusCode)
        {
            if (string.IsNullOrEmpty(rawContent))
            {
                Console.WriteLine("Response was successful but empty");
                return favoris;
            }

            return await response.Content.ReadFromJsonAsync<Favoris>();
        }

        Console.WriteLine($"Request failed with status: {response.StatusCode}");
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in AddFavoris: {ex.Message}");
        return null;
    }
}
    }
}