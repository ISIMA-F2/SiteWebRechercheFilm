using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FilmFront.Components.SharedModels;

namespace FilmFront.Components.SharedModels
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        private bool loggedIn = false;
        public bool LoggedIn
        {
            get { return loggedIn; }
            set
            {
                if (loggedIn != value)
                {
                    loggedIn = value;
                    OnLoginStatusChanged?.Invoke();
                }
            }
        }

        public event Action OnLoginStatusChanged;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResult> Login(Userinfo userinfo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/login", userinfo);

            if (response.IsSuccessStatusCode)
            {
                LoggedIn = true;
                return await response.Content.ReadFromJsonAsync<AuthResult>();
            }
            else
            {
                LoggedIn = false;
                return null;
            }
        }

        public async Task<Userinfo> Register(Userinfo userinfo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/register", userinfo);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Userinfo>();
            }
            else
            {
                return null;
            }
        }

        public void Logout()
        {
            LoggedIn = false;
        }
    }
}
