using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace RegistroLibros.PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public sealed class DevolucionesE2ETests : PageTest
{
    private static readonly string BaseUrl =
        Environment.GetEnvironmentVariable("PLAYWRIGHT_BASE_URL") ?? "http://localhost:5170";

    [Test]
    public async Task Index_Muestra_El_Registro_De_Devoluciones()
    {
        await NavegarAlIndexAsync();

        await Expect(Page).ToHaveTitleAsync(new Regex("^Registro de Devoluciones$"));
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Registro de Devoluciones" }))
            .ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Crear" }))
            .ToHaveAttributeAsync("href", "/Devolucion/Create");
    }

    [Test]
    public async Task Crear_Guarda_Y_Muestra_Una_Devolucion()
    {
        var filaDevolucion = await CrearDevolucionAsync();

        try
        {
            await Expect(filaDevolucion).ToBeVisibleAsync();
        }
        finally
        {
            if (await filaDevolucion.CountAsync() > 0)
            {
                await EliminarDevolucionAsync(filaDevolucion);
            }
        }
    }

    [Test]
    public async Task Eliminar_Quita_La_Devolucion_Del_Index()
    {
        var filaDevolucion = await CrearDevolucionAsync();

        await EliminarDevolucionAsync(filaDevolucion);

        await Expect(filaDevolucion).Not.ToBeVisibleAsync();
    }

    private async Task NavegarAlIndexAsync()
    {
        await Page.GotoAsync($"{BaseUrl.TrimEnd('/')}/Devolucion/Index");
    }

    private async Task<ILocator> CrearDevolucionAsync()
    {
        await NavegarAlIndexAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Crear" }).ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex("/Devolucion/Create$"));
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Crear Devolución" }))
            .ToBeVisibleAsync();

        var prestamo = await SeleccionarPrestamoDisponibleAsync();
        await Page.Locator("input[type='date']")
            .FillAsync(DateTime.Today.ToString("yyyy-MM-dd"));
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar" }).ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex("/Devolucion/Index$"));
        var filaDevolucion = BuscarFilaPorPrestamo(prestamo.Id);
        await Expect(filaDevolucion).ToBeVisibleAsync();

        return filaDevolucion;
    }

    private async Task<(int Id, string Libro)> SeleccionarPrestamoDisponibleAsync()
    {
        var selectorPrestamos = Page.GetByRole(AriaRole.Combobox);
        var opcionesPrestamos = selectorPrestamos.Locator("option:not([disabled])");
        var cantidadPrestamos = await opcionesPrestamos.CountAsync();
        Assert.That(cantidadPrestamos, Is.GreaterThan(0),
            "Se necesita al menos un préstamo activo para crear una devolución.");

        var prestamosDisponibles = new List<(int Id, string Libro)>();
        for (var indice = 0; indice < cantidadPrestamos; indice++)
        {
            var opcion = opcionesPrestamos.Nth(indice);
            var textoOpcion = (await opcion.InnerTextAsync()).Trim();
            var coincidenciaLibro = Regex.Match(textoOpcion, @"\((?<libro>.+)\)$");
            Assert.That(coincidenciaLibro.Success, Is.True,
                $"No se pudo identificar el libro del préstamo: {textoOpcion}");

            var valorId = await opcion.GetAttributeAsync("value");
            Assert.That(int.TryParse(valorId, out var prestamoId), Is.True,
                "El préstamo disponible debe tener un identificador válido.");

            prestamosDisponibles.Add((prestamoId, coincidenciaLibro.Groups["libro"].Value));
        }

        var indicePrestamoSeleccionado = prestamosDisponibles.FindIndex(actual =>
            prestamosDisponibles.Count(otro =>
                string.Equals(otro.Libro, actual.Libro, StringComparison.OrdinalIgnoreCase)) == 1);
        Assert.That(indicePrestamoSeleccionado, Is.GreaterThanOrEqualTo(0),
            "Se necesita un préstamo activo cuyo libro no tenga otro préstamo activo.");

        var prestamoSeleccionado = prestamosDisponibles[indicePrestamoSeleccionado];
        await selectorPrestamos.SelectOptionAsync(prestamoSeleccionado.Id.ToString());
        return prestamoSeleccionado;
    }

    private ILocator BuscarFilaPorPrestamo(int prestamoId)
    {
        return Page.GetByRole(AriaRole.Row, new()
        {
            NameRegex = new Regex($@"^\s*\d+\s+{prestamoId}\b")
        });
    }

    private async Task EliminarDevolucionAsync(ILocator filaDevolucion)
    {
        await filaDevolucion
            .Locator("button[data-bs-target='#modalEliminarDevolucion']")
            .ClickAsync();

        var modalEliminar = Page.Locator("#modalEliminarDevolucion");
        await Expect(modalEliminar).ToBeVisibleAsync();
        await modalEliminar.GetByRole(AriaRole.Button, new() { Name = "Sí, eliminar" }).ClickAsync();
        await Expect(Page.GetByText("Devolución eliminada correctamente.", new() { Exact = true }))
            .ToBeVisibleAsync();
    }
}
