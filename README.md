# Tarea - Sistema de Registro de Libros

## Datos del Estudiante

- **Estudiante:** Jeison Amparo Abreu
- **Matrícula:** 1000-5296

## Descripción

Proyecto web desarrollado en Blazor con .NET 10 para gestionar el registro de un catálogo de libros. La aplicación utiliza una base de datos SQlServer gestionada a través de Entity Framework Core y cuenta con un diseño de interfaz basado en Bootstrap.

## Requerimientos de la Base de Datos

La tabla principal del sistema está nombrada como `Libros` y contiene los siguientes atributos:

- LibroId
- Titulo
- Autor
- AnoPublicacion

Adicionalmente, se cuenta con una tabla nombrada como `Estudiantes` con los siguientes atributos:

- EstudianteId
- Nombres
- Direccion
- Email
- FechaNacimiento

## Validaciones Implementadas

- Todos los campos de los formularios son estrictamente obligatorios.
- No se permite el registro de dos libros con el mismo título exacto.
- No se permite el registro de dos estudiantes con el mismo nombre.
- Si el usuario intenta guardar un duplicado, el sistema bloquea la acción y muestra un mensaje de error en pantalla.

## Cómo ejecutar el proyecto

1. Clona el repositorio:

```bash
git clone https://github.com/Je1sonAmparo/RegistroLibros.git
```

2. Abre la solución en Visual Studio.
3. Asegúrate de tener instalado el paquete de Entity Framework Core y Blazor Bootstrap.