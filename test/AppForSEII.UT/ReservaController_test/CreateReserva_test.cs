using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDetailDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.ReservaController_test
{
    public class CreateReserva_test : AppForSEII4SqliteUT
    {
        private const string _nombreUsuario = "Elena";
        private const string _apellidosUsuario = "Navarro Martínez";
        private const string _dni = "12345678A";
        private const string _email = "elena.navarro@uclm.es";
        private const string _telefono = "666555444";
        private const string _datosPago = "1234-5678-9012-3456";

        private const string _pista1Nombre = "Pista de tenis 1";
        private const string _pista1TipoDeporte = "Tenis";
        private static readonly DateTime _fechaReserva = DateTime.Today.AddDays(1);

        public CreateReserva_test()
        {
            var tiposDeporte = new List<TipoDeporte>()
            {
                new TipoDeporte { Id = 1, Nombre = _pista1TipoDeporte },
                new TipoDeporte { Id = 2, Nombre = "Pádel" }
            };

            var pistas = new List<Pista>()
            {
                new Pista { IdPista = 1, NombrePista = _pista1Nombre, NPersonas = "4", Precio = 10.0, Stock = 3, TipoDeporte = tiposDeporte[0] },
                new Pista { IdPista = 2, NombrePista = "Pista de pádel 1", NPersonas = "4", Precio = 12.0, Stock = 1, TipoDeporte = tiposDeporte[1] }
            };

            var reserva = new Reserva(
                1,
                _apellidosUsuario,
                _dni,
                _fechaReserva,
                MetodoPago.Tarjeta,
                _nombreUsuario,
                12.0,
                new List<PistaReservada>()
            );

            reserva.PistasReservadas.Add(new PistaReservada
            {
                IdPista = 2,
                Pista = pistas[1],
                IdReserva = 1,
                Reserva = reserva,
                Cantidad = 1,
                Precio = 12,
                Observaciones = "Sin observaciones"
            });

            _context.AddRange(tiposDeporte);
            _context.AddRange(pistas);
            _context.Add(reserva);
            _context.SaveChanges();
        }

        private static ReservaForCreateDTO CrearReservaDTO(List<ReservaItemDTO> pistas, DateTime fechaReserva, string nombre = _nombreUsuario)
        {
            return new ReservaForCreateDTO(
                nombre,
                _apellidosUsuario,
                _dni,
                _email,
                _telefono,
                MetodoPago.Tarjeta,
                pistas,
                _datosPago,
                fechaReserva
            );
        }

        private static ReservaItemDTO CrearItem(int idPista, string? observaciones = null)
        {
            return new ReservaItemDTO(idPista, "", "", "4", 0.0, observaciones);
        }

       public static IEnumerable<object[]> TestCasesFor_CreateReserva()
        {
            var reservaSinPistas = CrearReservaDTO(new List<ReservaItemDTO>(), _fechaReserva);
            var reservaFechaPasada = CrearReservaDTO(new List<ReservaItemDTO>() { CrearItem(1) }, DateTime.Today.AddDays(-1));
            var reservaPistaRepetida = CrearReservaDTO(new List<ReservaItemDTO>() { CrearItem(1), CrearItem(1) }, _fechaReserva);
            var reservaPistaNoExiste = CrearReservaDTO(new List<ReservaItemDTO>() { CrearItem(99) }, _fechaReserva);
            var reservaPistaSinStock = CrearReservaDTO(new List<ReservaItemDTO>() { CrearItem(2) }, _fechaReserva);

            var allTests = new List<object[]>
            {
                new object[] { reservaSinPistas, "¡Error!" },
                new object[] { reservaFechaPasada, "¡Error! La fecha de su reserva no puede" },
                new object[] { reservaPistaRepetida, "¡Error! Hay pistas repetidas en la solici" },
                new object[] { reservaPistaNoExiste, "¡Error! La pista con ID '99' no existe." },
                new object[] { reservaPistaSinStock, "¡Error! La pista 'Pista de pádel 1' no es" }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReserva))]
        public async Task CreateReserva_Error_test(ReservaForCreateDTO reservaDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var result = await controller.CreateReserva(reservaDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    [Trait("Database", "WithoutFixture")]
        public async Task CreateReserva_Success_test()
        {
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var reservaDTO = CrearReservaDTO(
                new List<ReservaItemDTO>() { CrearItem(1, "Necesito iluminación") },
                _fechaReserva
            );

            // Asegúrate de que NPersonas ("4") y Precio (10.0) sean los mismos
            // que pusiste al crear la Pista con IdPista = 1 en el constructor de CreateReserva_test
            var pista1 = _context.Pistas.Find(1)!;

            var expectedReservaDetailDTO = new ReservaDetailDTO(
                2,
                _nombreUsuario,
                _apellidosUsuario,
                _dni,
                _email,
                _telefono,
                MetodoPago.Tarjeta,
                pista1.Precio,
                new List<ReservaItemDTO>()
                {
                    new ReservaItemDTO(1, pista1.NombrePista, _pista1TipoDeporte, pista1.NPersonas, pista1.Precio, "Necesito iluminación")
                },
                _datosPago,
                _fechaReserva
            );

            var result = await controller.CreateReserva(reservaDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualReservaDetailDTO = Assert.IsType<ReservaDetailDTO>(createdResult.Value);

            Assert.Equal(expectedReservaDetailDTO, actualReservaDetailDTO);
            Assert.Equal(2, _context.Reservas.Count());
        }
    }
}