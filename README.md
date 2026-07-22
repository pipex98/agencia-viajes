# Agencia de viajes - UltraGroup 

Este repositorio contiene la solución a la **Prueba Técnica para Back End Developer** de **UltraGroup**. para una plataforma moderna de alojamiento, escalable y mantenible para la gestión de catálogos de hoteles, habitaciones y reservas de una agencia de viajes.

## Stack Tecnologico y Arquitectura

- **Lenguaje y Framework:** C# con **.NET 10**.
- **Base de Datos:** **SQL Server** (Persistencia Relacional).
- **Justificación:** Se eligió un motor relacional debido a la naturaleza transaccional del dominio (evitar sobreventas de habitaciones mediante el uso de transacciones ACID) y la fuerte relación entre las entidades de negocio. 
- **Trade-offs:** Sacrificamos la flexibilidad de esquema de una NoSQL en favor de la consistencia e integridad de los datos. En un escenario de alta concurrencia de lecturas globales, se podría migrar el catálogo de búsqueda a un motor de búsqueda como Elasticsearch o implementar Redis como capa de caché.
- **Documentación de API:** OpenAPI / Scalar.

## Arquitectura de la Solucion (Clean Architecture)

Para garantizar el desacoplamiento, la mantenibilidad y la testeabilidad del sistema, se ha estructurado el proyecto bajo los principios de **Clean Architecture**:

1.  **Domain:** Contiene las entidades de negocio (Agente, Ciudad, ComisionReserva, DetalleReserva, Genero, Habitacion, Hotel, Huesped, Reserva, TipoDocumento, TipoHabitacion ).
2.  **Application:** Contiene los casos de uso (Commands/Queries con patrón CQRS a través de MediatR), validaciones, interfaces externas, servicios de aplicacion
3.  **Infrastructure:** Contiene el acceso a datos (Entity Framework Core), repositorios, integraciones externas (envío de emails y generacion de tokens) y persistencia.
4.  **API:** Capa de presentación que expone los endpoints REST controlando la autenticación y el rate limiting.

## Diagrama de Arquitectura (Modelo C4)

                              +---------------------------+
                              |      Viajero / Agente     |
                              +-------------+-------------+
                                            |
                                            | (HTTPS - JSON/REST)
                                            v
                              +-------------+-------------+
                              |            API            |
                              |         (.NET 10)         |
                              +------+-------------+------+
                                            |             
                                            | (Persistencia SQL)             
                                            v             
                                    +------+-----+     
                                    | PostgreSQL |     
                                    +------------+     


## Requisitos previos

* .NET 10 SDK (https://dotnet.microsoft.com/download).

## Instrucciones de Ejecución Local

**1.** Clona el repositorio:
   git clone https://github.com/pipex98/agencia-viajes

**2.** Ejecuta el script agencia-viajes.sql

**3.** Configurar la cadena de conexion

**4** Configurar los secretos de usuarios

**4.** cd AgenciaViajes

**5.** dotnet run --project AgenciaViajes.API

La API estará disponible en https://localhost:7001 (o el puerto configurado) y la interfaz de Scalar en https://localhost:7236/scalar/v1.  
                        
##  Retos Técnicos Abordados y Soluciones

 ### Seguridad en el Backend

1. **Autenticación & Autorización:** Implementación de JWT (JSON Web Tokens) para proteger el endpoint de reservar habitacion (exclusivo para viajeros).  

2. **Proteccion contra inyeccion:** Uso de entity framework para proteger las consultas de una inyeccion SQL

3. **Rate Limiting:** Configuración de middleware nativo de .NET para mitigar ataques de denegación de servicio (DoS) en todos los endpoints.  

4. **Validación de Inputs:** Uso de FluentValidation en la capa de aplicación para sanitizar y verificar la estructura de las solicitudes antes de procesarlas.

5. **Secretos de usuario:** Uso seguro de secretos para la generacion de tokens y el envio de correos. 

6. **Https enforcement:** Configuracion de middlewares nativo de .net para interceptar todas las solicitudes entrantes que llegan a través del puerto no seguro (HTTP, normalmente el puerto 80) y las redirige automáticamente al puerto seguro (HTTPS, normalmente el puerto 443) con un código de estado HTTP 307 y añadir un encabezado de seguridad a las respuestas HTTPS.

7. **logging de eventos de seguridad:** para manejar una trazabilidad de lo que va sucediento dentro del sistema.

### Test Driven Development 

Uso del patron red-green-refactor para escribir pruebas escalables, mantenibles y faciles de entender, en este caso no se implemento refactorizacion porque de entrada el codigo se escribio optimizado.

### Clean Code y Patrones de diseño 

Uso del patron repository para centralizar las interacciones con la base de datos y poder cambiar el motor sin afectar el resto de las capas. uso del patron mediator para centralizar la comunicacion compleja entre multiples objetos a través de un único objeto mediator. Uso del patron CQRS para separar las operaciones de escritura (comandos) de las operaciones de lectura (consultas)

### Uso de IA en el Desarrollo

**Herramientas utilizadas**: GitHub Copilot.

**Casos de uso**: para autocompletado de codigo y traducir consultas SQL a entity framework .  

**Verificación de calidad**: Todo código generado por IA fue revisado manualmente línea por línea.  

## Pruebas y Colección de Postman

En el proyecto AgenciaViajes.API.Tests se incluye una prueba unitaria para un flujo crítico de negocio **(Crear Reserva)**.  

En la raíz del proyecto encontrarás el archivo AgenciaViajes-API.postman_collection.json con todas las peticiones listas para importar en Postman, organizadas por:  

**Habitaciones** - **Hoteles** - **Huespedes** - **Auth**

**Flujo del Agente:** Crear hotel, Modificar hotel, Asignar habitación, Modificar precios o informacion de la habitacion, Listar Reservas, Habilitar Hotel, Desabilitar Hotel, Habilitar Habitacion, Desabilitar Habitacion,  

**Flujo del Viajero:** Registrarse, Autenticarse, Buscar habitaciones disponibles, Crear Reserva.  

