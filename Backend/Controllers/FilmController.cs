using Microsoft.AspNetCore.Mvc;
using TrackerDeFavorisApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TrackerDeFavorisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {   
        private readonly FilmContext _Filmcontext;
        

        public FilmController(FilmContext ctx){
            _Filmcontext = ctx;
        }
        // GET: api/<FilmController>
        [HttpGet("Films")]
    public async Task<ActionResult<List<Film>>> GetFilms()
    {
        var LesFilms = await _Filmcontext.Films.ToListAsync();

        if (LesFilms == null || LesFilms.Count == 0)
        {
            return NotFound("No Films found.");
        }

        return Ok(LesFilms) ;
    }

        // GET api/<FilmController>/5
        [HttpGet("Finding film")]
        public async Task<ActionResult<List<Film>>> GetUserWords(string Text){
            var LesFilmsxt = await _Filmcontext.Films.Where(f => f.Title.Contains(Text)).ToListAsync();
            if (LesFilmsxt == null || LesFilmsxt.Count == 0){
                return NotFound("Pas de films");
            }
            else{
                return Ok($"Voici les films {LesFilmsxt}");
            }
        }
        [HttpGet("Id Films")]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilm([FromQuery] int[] ids){
            var LesFilmid = await _Filmcontext.Films.Where(f => ids.Contains(f.Id)).ToListAsync();
            if (LesFilmid == null || LesFilmid.Count == 0){
                return NotFound("Pas de films");
            }
            return Ok(LesFilmid);

        }
    }
}