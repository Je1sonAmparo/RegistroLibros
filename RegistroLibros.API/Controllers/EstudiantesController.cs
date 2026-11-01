using Mapster;
using Microsoft.AspNetCore.Mvc;
using RegistroLibros.API.DTOs.Estudiantes;
using RegistroLibros.Data.Models;
using RegistroLibros.Services;

namespace RegistroLibros.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstudiantesController(EstudiantesService estudiantesService) : ControllerBase
{
    // GET: api/Estudiantes
    [HttpGet]
    public async Task<ActionResult<List<EstudianteSalidaDto>>> Get()
    {
        var estudiantesResult = await estudiantesService.GetList(e => true);

        if (estudiantesResult.IsFailure)
        {
            return BadRequest(estudiantesResult.Error);
        }

        return Ok(estudiantesResult.Value.Adapt<List<EstudianteSalidaDto>>());
    }

    // GET: api/Estudiantes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstudianteSalidaDto>> GetById(int id)
    {
        var estudianteResult = await estudiantesService.Buscar(id);

        if (estudianteResult.IsFailure)
        {
            return BadRequest(estudianteResult.Error);
        }

        if (estudianteResult.Value == null)
        {
            return NotFound();
        }

        return Ok(estudianteResult.Value.Adapt<EstudianteSalidaDto>());
    }

    // POST: api/Estudiantes
    [HttpPost]
    public async Task<ActionResult<EstudianteSalidaDto>> Post([FromBody] EstudianteEntradaDto estudianteDto)
    {
        var estudiante = estudianteDto.Adapt<Estudiantes>();
        var resultado = await estudiantesService.Guardar(estudiante);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = estudiante.EstudianteId },
            estudiante.Adapt<EstudianteSalidaDto>());
    }

    // PUT: api/Estudiantes/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] EstudianteEntradaDto estudianteDto)
    {
        var estudianteActual = await estudiantesService.Buscar(id);

        if (estudianteActual.IsFailure)
        {
            return BadRequest(estudianteActual.Error);
        }

        if (estudianteActual.Value == null)
        {
            return NotFound();
        }

        var estudiante = estudianteDto.Adapt(estudianteActual.Value);
        var resultado = await estudiantesService.Guardar(estudiante);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return NoContent();
    }

    // DELETE: api/Estudiantes/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var estudiante = await estudiantesService.Buscar(id);

        if (estudiante.IsFailure)
        {
            return BadRequest(estudiante.Error);
        }

        if (estudiante.Value == null)
        {
            return NotFound();
        }

        var resultado = await estudiantesService.Eliminar(id);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        var eliminacion = (Aplicada1.Core.Result<bool>)resultado;
        if (!eliminacion.Value)
        {
            return Conflict("No se puede eliminar el estudiante porque tiene préstamos asociados.");
        }

        return NoContent();
    }
}
