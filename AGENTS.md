# RestApi · Restaurante con mesero virtual (ITQ)

## Stack
- C# 14, .NET 10, ASP.NET Core Minimal API
- MySQL con ADO.NET (MySqlConnector)
- Pruebas manuales con archivos .http

## Arquitectura (no romper)
- Entidades/ → clases simples POCO, sin lógica
- Datos/     → interfaces y repositorios, único lugar con SQL
- Negocio/   → servicios: validaciones y reglas de negocio
- Web/       → endpoints con MapGroup y Respuestas.Manejar, sin SQL
- Los servicios dependen de interfaces (IPlatoRepositorio, ICategoriaRepositorio, IProductoRepositorio)

## Convenciones
- Métodos async terminan en Async
- Borrado lógico: Disponible = 0, nunca DELETE físico en Platos
- HTTP: 201 crear, 204 eliminar, 400 inválido,
        404 no existe, 409 conflicto
- Cadena de conexión desde .env o appsettings.json (ConnectionStrings__Restaurante)

## Prohibido
- Concatenar valores en SQL (usar siempre consultas parametrizadas)
- Colocar lógica de base de datos fuera de Datos/
- Subir .env, bin/ u obj/ a GitHub
