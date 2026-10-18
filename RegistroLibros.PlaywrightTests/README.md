# Pruebas E2E de RegistroLibros

Estas pruebas usan NUnit y Playwright para verificar la página de índice de
Devoluciones. Están separadas en tres pruebas: cargar el index, crear una
devolución y eliminar una devolución. La prueba de creación limpia el registro
que genera; la prueba de eliminación crea su propio registro antes de borrarlo.

## Requisitos

- .NET SDK 10.
- La aplicación RegistroLibros en ejecución y conectada a una base de datos SQL
  configurada con la cadena `ConnectionStrings:SqlConStr`.
- Al menos un préstamo activo cuyo libro no tenga otro préstamo activo. Las
  pruebas de creación y eliminación seleccionan automáticamente un préstamo
  que cumple esa condición.

Desde la raíz de la solución, inicia la aplicación con el perfil HTTP:

```powershell
dotnet run --project RegistroLibros\RegistroLibros.csproj --launch-profile http
```

El perfil `http` escucha en `http://localhost:5170`, que es la URL predeterminada
para las pruebas.

## Compilar, instalar Chromium y ejecutar

```powershell
dotnet build RegistroLibros.PlaywrightTests\RegistroLibros.PlaywrightTests.csproj
powershell -ExecutionPolicy Bypass -File RegistroLibros.PlaywrightTests\bin\Debug\net10.0\playwright.ps1 install chromium
dotnet test RegistroLibros.PlaywrightTests\RegistroLibros.PlaywrightTests.csproj
```

Para probar otra instancia, define `PLAYWRIGHT_BASE_URL` con la URL raíz de la
aplicación antes de ejecutar `dotnet test`. Por ejemplo:

```powershell
$env:PLAYWRIGHT_BASE_URL = "https://localhost:7093"
dotnet test RegistroLibros.PlaywrightTests\RegistroLibros.PlaywrightTests.csproj
```
