using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Data.Models;
using RegistroLibros.Services;

namespace RegistroLibros.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrestamosController(PrestamosService prestamosService) : ControllerBase
{
    // GET: api/Prestamos
    [HttpGet]
    public async Task<ActionResult<List<Prestamos>>> Get()
    {
        var prestamosResult = await prestamosService.GetList(p => true);

        if (prestamosResult.IsFailure)
        {
            return BadRequest(prestamosResult.Error);
        }

        return Ok(prestamosResult.Value);
    }

    // GET: api/Prestamos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Prestamos>> Get(int id)
    {
        var prestamoResult = await prestamosService.Buscar(id);

        if (prestamoResult.IsFailure)
        {
            return BadRequest(prestamoResult.Error);
        }

        if (prestamoResult.Value == null)
        {
            return NotFound();
        }

        return Ok(prestamoResult.Value);
    }

    // POST: api/Prestamos
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Prestamos prestamo)
    {
        var resultado = await prestamosService.Guardar(prestamo);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return Created();
    }

    // PUT: api/Prestamos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Prestamos prestamo)
    {
        if (id != prestamo.PrestamoId)
        {
            return BadRequest();
        }

        var prestamoActual = await prestamosService.Buscar(id);

        if (prestamoActual.IsFailure)
        {
            return BadRequest(prestamoActual.Error);
        }

        if (prestamoActual.Value == null)
        {
            return NotFound();
        }

        var resultado = await prestamosService.Guardar(prestamo);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return NoContent();
    }

    // DELETE: api/Prestamos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var prestamo = await prestamosService.Buscar(id);

        if (prestamo.IsFailure)
        {
            return BadRequest(prestamo.Error);
        }

        if (prestamo.Value == null)
        {
            return NotFound();
        }

        var resultado = await prestamosService.Eliminar(id);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        var eliminacion = (Aplicada1.Core.Result<bool>)resultado;
        if (!eliminacion.Value)
        {
            return Conflict("No se pudo eliminar el préstamo.");
        }

        return NoContent();
    }
}
