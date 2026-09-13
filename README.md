# Tarea - Sistema de Registro de Libros

## Datos del Estudiante

- **Estudiante:** Jeison Amparo Abreu
- **Matrícula:** 1000-5296

## Descripción

Proyecto web desarrollado en Blazor con .NET 8 para gestionar el registro de un catálogo de libros. La aplicación utiliza una base de datos SQLite gestionada a través de Entity Framework Core y cuenta con un diseño de interfaz basado en Bootstrap.

## Requerimientos de la Base de Datos

La tabla principal del sistema está nombrada como `Libros` y contiene los siguientes atributos:

- LibroId
- Titulo
- Autor
- AnoPublicacion

## Validaciones Implementadas

- Todos los campos del formulario son estrictamente obligatorios.
- No se permite el registro de dos libros con el mismo título exacto.
- Si el usuario intenta guardar un duplicado, el sistema bloquea la acción y muestra un mensaje de error en pantalla.

## Entregables

- **Capturas de pantalla (10%):** Imágenes que demuestran la funcionalidad del registro y la vista principal de los datos.
- **Repositorio en GitHub (50%):** Enlace público con el código fuente del proyecto.
- **Aplicación en Somee (40%):** Sistema publicado y funcionando en la plataforma Somee para su evaluación en línea.

## Cómo ejecutar el proyecto localmente

1. Clona el repositorio:

```bash
git clone https://github.com/Je1sonAmparo/RegistroLibros.git
```

2. Abre la solución en Visual Studio.
3. Asegúrate de tener instalado el paquete de Entity Framework Core y Blazor Bootstrap.
4. Ejecuta el proyecto. La base de datos SQLite se gestionará de manera local para permitir las pruebas de guardado y validación de títulos duplicados.
