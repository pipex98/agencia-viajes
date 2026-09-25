using AgenciaViajes.Application.Dto.DetalleReserva;
using AgenciaViajes.Application.Dto.Reserva;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace AgenciaViajes.API.Tests
{
    public class HabitacionesControllerTests : IClassFixture<AgenciaViajesApiFactory>
    {
        readonly HttpClient _client;

        public HabitacionesControllerTests(AgenciaViajesApiFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Utils.GetJwtToken());
        }

        [Fact]
        public async Task CrearReserva()
        {
            var dto = new AddReservaDto
            {
                IdHuesped = 1,
                IdHabitacion = 2,
                FechaIngreso = new DateTime(2026, 7, 23, 17, 30, 00),
                FechaSalida = new DateTime(2026, 8, 15, 14, 00, 00),
                ContactoEmergencia = "Martha Jimenez 3234643253",
                DetalleReservas = new List<AddDetalleReservaDto>
                {
                    new AddDetalleReservaDto
                    {
                        Concepto = "Noches",
                        Cantidad = 7,
                        PrecioUnitario = 360450.00m
                    },
                    new AddDetalleReservaDto
                    {
                        Concepto = "Desayuno",
                        Cantidad = 28,
                        PrecioUnitario = 36045.00m
                    },
                    new AddDetalleReservaDto
                    {
                        Concepto = "Recogida en el aeropuerto",
                        Cantidad = 1,
                        PrecioUnitario = 288360.00m
                    }
                }
            };

            var response = await _client.PostAsJsonAsync("/api/habitaciones/reservar",
                dto,
                TestContext.Current.CancellationToken
            );

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
