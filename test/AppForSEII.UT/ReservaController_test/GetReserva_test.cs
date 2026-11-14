using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDetailDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.ReservaController_test
{
    public class GetReserva_test : AppForSEII4SqliteUT
    {
        private const string _nombreCliente = "Elena";
        private const string _apellidosCliente = "Navarro Martínez";
        private const string _dniCliente = "12345678A";
        private const string _email = "elena.navarro@uclm.es";
        private const string _telefono = "666555444";
        private const string _datosPago = "1234-5678-9012-3456";
        private static readonly DateTime _fechaReserva = DateTime.Today.AddDays(1);

        public GetReserva_test()
        {
            var tiposDeporte = new List<TipoDeporte>()
            {
                new TipoDeporte { Id = 1, Nombre = "Tenis" },
                new TipoDeporte { Id = 2, Nombre = "Pádel" }
            };

            var pistas = new List<Pista>()
            {
                new Pista { IdPista = 1, NombrePista = "Pista de tenis 1", NPersonas = "4", Precio = 10.0, Stock = 3, TipoDeporte = tiposDeporte[0] },
                new Pista { IdPista = 2, NombrePista = "Pista de pádel 1", NPersonas = "4", Precio = 12.0, Stock = 1, TipoDeporte = tiposDeporte[1] }
            };

            var reserva = new Reserva(
                1,
                _apellidosCliente,
                _dniCliente,
                _fechaReserva,
                MetodoPago.Tarjeta,
                _nombreCliente,
                12.0,
                new List<PistaReservada>()
            );

            reserva.PistasReservadas.Add(new PistaReservada
            {
                IdReserva = 1,
                IdPista = 2,
                Pista = pistas[1],
                Reserva = reserva,
                Cantidad = 1,
                Precio = 12,
                Observaciones = "Turno de tarde"
            });

            _context.TipoDeporte.AddRange(tiposDeporte);
            _context.Pistas.AddRange(pistas);
            _context.Reservas.Add(reserva);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReserva_OK_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            var expectedReservaDetailDTO = new ReservaDetailDTO(
                1,
                _nombreCliente,
                _apellidosCliente,
                _dniCliente,
                _email,
                _telefono,
                MetodoPago.Tarjeta,
                12,
                new List<ReservaItemDTO>()
                {
                    new ReservaItemDTO(2, "Pista de pádel 1", "Pádel", "4", 12.0, "Turno de tarde")
                },
                _datosPago,
                _fechaReserva
            );

            // Act
            var result = await controller.GetReserva(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ReservaDetailDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualReservaDetailDTO = Assert.IsType<ReservaDetailDTO>(okResult.Value);

            Assert.Equal(expectedReservaDetailDTO.Id, actualReservaDetailDTO.Id);
            Assert.Equal(expectedReservaDetailDTO.NombreUsuario, actualReservaDetailDTO.NombreUsuario);
            Assert.Equal(expectedReservaDetailDTO.ApellidosUsuario, actualReservaDetailDTO.ApellidosUsuario);
            Assert.Equal(expectedReservaDetailDTO.Dni, actualReservaDetailDTO.Dni);
            Assert.Equal(expectedReservaDetailDTO.PrecioTotal, actualReservaDetailDTO.PrecioTotal);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReserva_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReservaController>>();
            ILogger<ReservaController> logger = mock.Object;
            var controller = new ReservaController(_context, logger);

            // Act
            var result = await controller.GetReserva(99);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ReservaDetailDTO>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}