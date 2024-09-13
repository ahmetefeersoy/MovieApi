using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Film;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/films")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _filmRepo;

        public FilmController(IFilmRepository filmRepo)
        {
            _filmRepo = filmRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {   
                if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var films = await _filmRepo.GetAllAsync(query);
            var filmDto = films.Select(s => s.ToFilmDto()).ToList();
            return Ok(filmDto);
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetByName([FromRoute]string name)
        {       if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var films = await _filmRepo.GetByNameAsync(name);
            if (!films.Any())
            {
                return NotFound();
            }
            var filmDto = films.Select(s => s.ToFilmDto());
            return Ok(filmDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {       if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var film = await _filmRepo.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return Ok(film.ToFilmDto());
        }

      [HttpPost]
public async Task<IActionResult> Create([FromBody] CreateFilmRequestDto filmDto)
{   
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var filmModel = filmDto.ToFilmFromCreateDto();
    await _filmRepo.CreateAsync(filmModel);

    // Düzeltme: Film oluşturulmadan önce filmModel.Id ayarlanmalıdır.
    return CreatedAtAction(nameof(GetById), new { id = filmModel.Id }, filmModel.ToFilmDto());
}


       [HttpPut("{id:int}")]
public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateFilmRequestDto updateDto)
{   
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var filmModel = await _filmRepo.UpdateAsync(id, updateDto);
    if (filmModel == null)
    {
        return NotFound();
    }
    
    return Ok(filmModel.ToFilmDto()); // Düzeltme: ToFilmDto metodunu kullanın
}


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {   
                if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var filmModel = await _filmRepo.DeleteAsync(id);
            if (filmModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}