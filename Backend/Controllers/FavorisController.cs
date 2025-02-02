using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackerDeFavorisApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace TrackerDeFavorisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly FavorisContext _Favoriscontext;


        public FavorisController(FavorisContext ctx)
        {
            _Favoriscontext = ctx;
        }

[HttpPost("add-favorite")]
public async Task<ActionResult<Favoris>> PostFavoris(Favoris favoris)
{
    if (string.IsNullOrEmpty(favoris.UserId))
    {
        return BadRequest("UserId cannot be empty");
    }

    try
    {
        var newFavoris = new Favoris
        {
            Id = favoris.Id,
            UserId = favoris.UserId,
            Film = favoris.Film
        };

        _Favoriscontext.Favoriss.Add(newFavoris);
        await _Favoriscontext.SaveChangesAsync();

        return CreatedAtAction(nameof(PostFavoris), new { id = newFavoris.Id }, newFavoris);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving favorite: {ex.Message}");
        return StatusCode(500, "An error occurred while saving the favorite");
    }
}
        [HttpDelete("Remove Favoris")]
        public async Task<IActionResult> DeleteFavoris(int id){
            var f = await _Favoriscontext.Favoriss.FindAsync(id);
            if (f == null){
                return NotFound("No film!!");
            }
            else{
                await _Favoriscontext.SaveChangesAsync();
                _Favoriscontext.Favoriss.Remove(f);
            }
            return Ok("Deleted!!");
        }
[HttpGet("List")]
public async Task<IActionResult> GetFavorites([FromQuery] string userId)
{
    var arr = await _Favoriscontext.Favoriss
        .Where(f => f.UserId == userId)
        .ToListAsync();

    if (arr.Any())
    {
        return Ok(arr);
    }
    return NotFound("Aucun favori trouvé pour l'utilisateur spécifié.");
}


    }
}