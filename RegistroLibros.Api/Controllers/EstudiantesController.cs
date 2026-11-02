
using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Api.DTOs;
using RegistroLibros.Data.Models;
using RegistroLibros.Services.Services;
using Mapster;

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController : ControllerBase
{
    private readonly EstudiantesService estudiantesService;

    public EstudiantesController(EstudiantesService estudiantesService)
    {
        this.estudiantesService = estudiantesService;
    }

    // GET: api/estudiantes
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await estudiantesService.GetList(e => e.EstudianteId > 0);
        var resultDto = result.Value.Adapt<List<EstudianteDto>>();
        return Ok(resultDto);
    }

    // GET: api/estudiantes/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await estudiantesService.Buscar(id);

        if (!result.IsSuccess || result.Value == null)
        {
            return NotFound();
        }

        var resultDto = result.Value.Adapt<EstudianteDto>();
        return Ok(resultDto);
    }

    // POST: api/estudiantes
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EstudianteDto estudianteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var estudiante = estudianteDto.Adapt<Estudiante>();
        estudiante.EstudianteId = 0;

        var result = await estudiantesService.Guardar(estudiante);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo agregar el estudiante.");
        }

        var resultDto = estudiante.Adapt<EstudianteDto>();
        return Ok(resultDto);
    }

    // PUT: api/estudiantes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] EstudianteDto estudianteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!await estudiantesService.Existe(id))
        {
            return NotFound();
        }

        var estudiante = estudianteDto.Adapt<Estudiante>();
        estudiante.EstudianteId = id;

        var result = await estudiantesService.Guardar(estudiante);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo actualizar el estudiante.");
        }

        var resultDto = estudiante.Adapt<EstudianteDto>();
        return Ok(resultDto);
    }

    // DELETE: api/estudiantes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await estudiantesService.Existe(id))
        {
            return NotFound();
        }

        var result = await estudiantesService.Eliminar(id);
        return Ok(result);
    }
}
