
using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Api.DTOs;
using RegistroLibros.Data.Models;
using RegistroLibros.Services.Services;
using Mapster;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController : ControllerBase
{
    private readonly PrestamosService prestamosService;

    public PrestamosController(PrestamosService prestamosService)
    {
        this.prestamosService = prestamosService;
    }

    // GET: api/prestamos
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await prestamosService.GetList(p => p.PrestamoId > 0);
        var resultDto = result.Value.Adapt<List<PrestamoDto>>();
        return Ok(resultDto);
    }

    // GET: api/prestamos/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await prestamosService.Buscar(id);

        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        var resultDto = result.Value.Adapt<PrestamoDto>();
        return Ok(resultDto);
    }

    // POST: api/prestamos
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PrestamoDto prestamoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var prestamo = prestamoDto.Adapt<Prestamo>();
        prestamo.PrestamoId = 0;

        var result = await prestamosService.Guardar(prestamo);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo guardar el préstamo.");
        }

        var resultDto = prestamo.Adapt<PrestamoDto>();
        return Ok(resultDto);
    }

    // PUT: api/prestamos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PrestamoDto prestamoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await prestamosService.Existe(id))
        {
            return NotFound();
        }

        var prestamo = prestamoDto.Adapt<Prestamo>();
        prestamo.PrestamoId = id;

        var result = await prestamosService.Guardar(prestamo);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo actualizar el préstamo.");
        }

        var resultDto = prestamo.Adapt<PrestamoDto>();
        return Ok(resultDto);
    }

    // DELETE: api/prestamos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await prestamosService.Existe(id))
        {
            return NotFound();
        }

        var result = await prestamosService.Eliminar(id);

        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo eliminar el préstamo.");
        }

        return NoContent();
    }
}
