using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography;
using TrackerDeFavorisApi.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Backend.Models;
namespace Backend.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserService(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<AuthResult?> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/login", loginRequest);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<AuthResult>();
        }
        public string HashPassword(string password)
        {
        // Pour rendre le hashage unique
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Séparer le salt et le hash stocké
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        // Recalculer le hash avec le même salt
        string hashToCompare = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return hashToCompare == parts[1];
    }


    public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = jwtSettings["Secret"] ?? throw new InvalidOperationException("La clé JWT n'est pas configurée.");
            var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("L'émetteur JWT n'est pas configuré.");
            var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("L'audience JWT n'est pas configurée.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("role", user.RoleType.ToString())            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}