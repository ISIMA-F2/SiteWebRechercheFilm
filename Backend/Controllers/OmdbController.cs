using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackerDeFavorisApi.Models;
using TrackerDeFavorisApi.Services;

[Route("api/[controller]")]
[ApiController]
public class OmdbController : ControllerBase
{
    private readonly OmdbService _omdbService;

    public OmdbController(OmdbService omdbService)
    {
        _omdbService = omdbService;
    }

    [HttpGet("search/{title}")]
    public async Task<IActionResult> SearchByTitle(string title)
    {
        var response = await _omdbService.SearchFilmsByTitleAsync(title);
        if (response == null)
            return NotFound("Films not found.");
        return Ok(response);
    }

    [HttpGet("import/{imdbId}")]
    public async Task<IActionResult> ImportFilmByImdbId(string imdbId)
    {
        var filmDetail = await _omdbService.GetFilmByImdbIdAsync(imdbId);
        if (filmDetail == null)
            return NotFound("Film not found.");

        // Logique pour ajouter le film à la base de données ici.
        return Ok(filmDetail);
    }
}
