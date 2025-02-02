using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using FilmFront.Components.SharedModels;

namespace FilmFront.Components.SharedModels
{
    public class AuthProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _sessionStorage;

        public AuthProvider(ProtectedLocalStorage protectedSessionStorage)
        {
            _sessionStorage = protectedSessionStorage;
        }

        public async Task LoginAuth(User user, string token)
        {
            try
            {
                await _sessionStorage.SetAsync("User", user);
                await _sessionStorage.SetAsync("Token", token);

                Console.WriteLine($"✅ User {user.Id} stored in session storage.");

                ClaimsPrincipal claim = GenerateClaimsPrincipal(user);
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claim)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during login: {ex.Message}");
            }
        }


        public ClaimsPrincipal GenerateClaimsPrincipal(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nom),
                new Claim(ClaimTypes.Role, user.RoleType.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "custom");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }

        public async Task Logout()
        {
            await _sessionStorage.DeleteAsync("User");
            await _sessionStorage.DeleteAsync("Token");

            var claimDisconnected = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimDisconnected)));
            
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userResult = await _sessionStorage.GetAsync<User>("User");

                if (userResult.Value != null)
                {
                    Console.WriteLine($"✅ User found in session storage: {userResult.Value.Id}");
                    var claim = GenerateClaimsPrincipal(userResult.Value);
                    return new AuthenticationState(claim);
                }
                else
                {
                    Console.WriteLine("⚠️ No user found in session storage.");
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is Exception)
            {
                Console.WriteLine($"❌ Error during GetAuthenticationStateAsync: {ex.Message}");
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

    }
}
