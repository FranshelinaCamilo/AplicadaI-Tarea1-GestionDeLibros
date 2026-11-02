using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Api.DTOs;
using RegistroLibros.Data.Models;
using RegistroLibros.Services.Services;
using Mapster;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistroLibros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController : ControllerBase
{
    private readonly LibrosService librosService;

    public LibrosController(LibrosService librosService)
    {
        this.librosService = librosService;
    }
    // GET: api/<LibrosController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await librosService.GetList(l => l.LibroId > 0);
        var resultDto = result.Value.Adapt<List<LibroDto>>();
        return Ok(resultDto);
    }

    // GET api/<LibrosController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await librosService.Buscar(id);
        var resultDto = result.Value.Adapt<LibroDto>();
        return Ok(resultDto);
    }

    // POST api/<LibrosController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LibroDto librodto)
    {
        var libro = librodto.Adapt<Libro>();
        libro.LibroId = 0;
        var result = await librosService.Guardar(libro);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo agregar el libro.");
        }
        var resultDto = libro.Adapt<LibroDto>();
        return Ok(resultDto);
    }

    // PUT api/<LibrosController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] LibroDto librodto)
    {
        var libro = librodto.Adapt<Libro>();
        libro.LibroId = id;
        var result = await librosService.Guardar(libro);

        if (!result.IsSuccess)
        {
            return BadRequest("No se pudo actualizar el libro.");
        }
        var resultDto = libro.Adapt<LibroDto>();
        return Ok(resultDto);
    }

    // DELETE api/<LibrosController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await librosService.Eliminar(id);
        return Ok(result);
    }
}