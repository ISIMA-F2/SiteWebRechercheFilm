using Microsoft.AspNetCore.Mvc;
using TrackerDeFavorisApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TrackerDeFavorisApi.Controllers
{
       
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _userManager;
         private readonly UserService _userService;
        public UserController(UserContext userManager, UserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userManager.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("User")]
        public async Task<ActionResult<List<string>>> GetUsers()
        {
            var usersprenom = await _userManager.Users.Select(u => u.Id).ToListAsync();

            if (usersprenom == null || usersprenom.Count == 0)
            {
                return NotFound("No users found.");
            }

            return Ok(usersprenom);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public async Task<ActionResult<string>> GetAdmins(){
            var admins = await _userManager.Users.Where(u => u.RoleType == 0).Select(u => u.Id).ToListAsync();
            if (admins == null || admins.Count == 0){
                return NotFound("No users Found.");
            }
            else{
                return Ok(admins);
            }

        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Register(Userinfo userinfo)
        {
            var existingUser = await _userManager.Users.FindAsync(userinfo.Login);
            if (existingUser != null)
            {
                return Conflict("Cet identifiant est déjà utilisé.");
            }

            var user = new User
            {
                Id = userinfo.Login,
                MotDePasse = _userService.HashPassword(userinfo.Password),
                RoleType = userinfo.Role 
            };

            _userManager.Users.Add(user);
            await _userManager.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { Id = user.Id }, user);
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == loginRequest.Login);
            if (user == null || !_userService.VerifyPassword(loginRequest.Password, user.MotDePasse))
            {
                return Unauthorized("Identifiants incorrects.");
            }

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { message = "Connexion réussie", user = user, token });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(string id, Userinfo userinfo)
        {
            var user = await _userManager.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Prenom = userinfo.Login;
            user.MotDePasse = _userService.HashPassword(userinfo.Password);
            _userManager.Entry(user).State = EntityState.Modified;

            try
            {
                await _userManager.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erreur de concurrence");
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _userManager.Users.Remove(user);
            await _userManager.SaveChangesAsync();

            return NoContent();
        }
}
}