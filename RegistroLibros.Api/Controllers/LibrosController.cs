using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Data.Models;
using RegistroLibros.Services.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private readonly LibrosService _libroService;

    public LibrosController(LibrosService libroService)
    {
        _libroService = libroService;
    }

    // GET: api/libros
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
    {
        var result = await _libroService.GetList(l => true);
        return Ok(result.Value);
    }

    // GET: api/libros/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetLibro(int id)
    {
        var result = await _libroService.Buscar(id);

        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    // POST: api/libros 
    [HttpPost]
    public async Task<ActionResult<Libro>> PostLibro(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _libroService.Guardar(libro);
        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo guardar el libro.");
        }

        return CreatedAtAction(nameof(GetLibro), new { id = libro.LibroId }, libro);
    }

    // PUT: api/libros/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLibro(int id, Libro libro)
    {
        if (id != libro.LibroId)
        {
            return BadRequest("El ID de la ruta no coincide con el ID del libro.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await _libroService.Existe(id))
        {
            return NotFound();
        }

        var result = await _libroService.Guardar(libro);
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar el libro.");
        }

        return NoContent();
    }

    // DELETE: api/libros/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibro(int id)
    {
        if (!await _libroService.Existe(id))
        {
            return NotFound();
        }

        var result = await _libroService.Eliminar(id);
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo eliminar el libro.");
        }

        return NoContent();
    }
}

