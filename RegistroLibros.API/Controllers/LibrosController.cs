using Microsoft.AspNetCore.Mvc;
using RegistroLibros.Data.Models;
using RegistroLibros.Services;

namespace RegistroLibros.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController(LibrosService librosService) : ControllerBase
{
    // GET: api/Libros
    [HttpGet]
    public async Task<ActionResult<List<Libros>>> Get()
    {
        var librosResult = await librosService.GetList(l => true);

        if (librosResult.IsFailure)
        {
            return BadRequest(librosResult.Error);
        }

        return Ok(librosResult.Value);
    }

    // GET: api/Libros/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Libros>> Get(int id)
    {
        var libroResult = await librosService.Buscar(id);

        if (libroResult.IsFailure)
        {
            return BadRequest(libroResult.Error);
        }

        if (libroResult.Value == null)
        {
            return NotFound();
        }

        return Ok(libroResult.Value);
    }

    // POST: api/Libros
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Libros libro)
    {
        var resultado = await librosService.Guardar(libro);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return Created();
    }

    // PUT: api/Libros/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Libros libro)
    {
        if (id != libro.LibroId)
        {
            return BadRequest();
        }

        var libroActual = await librosService.Buscar(id);

        if (libroActual.IsFailure)
        {
            return BadRequest(libroActual.Error);
        }

        if (libroActual.Value == null)
        {
            return NotFound();
        }

        var resultado = await librosService.Guardar(libro);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        return NoContent();
    }

    // DELETE: api/Libros/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await librosService.Buscar(id);

        if (libro.IsFailure)
        {
            return BadRequest(libro.Error);
        }

        if (libro.Value == null)
        {
            return NotFound();
        }

        var resultado = await librosService.Eliminar(id);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Error);
        }

        var eliminacion = (Aplicada1.Core.Result<bool>)resultado;
        if (!eliminacion.Value)
        {
            return Conflict("No se puede eliminar el libro porque tiene préstamos activos.");
        }

        return NoContent();
    }
}
