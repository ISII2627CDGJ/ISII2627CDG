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
        public GetReserva_test()
        {
            var tipo = new TipoDeporte { Id = 1, Nombre = "Fútbol" };
            var pista = new Pista { IdPista = 1, NombrePista = "Campo 1", NPersonas = "22", Precio = 50.0, Stock = 1, TipoDeporte = tipo };

            var usuario = new ApplicationUser(
    "1",
    "María",
    "García",
    "elena.navarro@uclm.es",
    "12345678A",
    25,
    "Mujer"
)
{
    PhoneNumber = "666555444"
};

        var reserva = new Reserva(
            1,
            DateTime.Today.AddDays(1),
             MetodoPago.Tarjeta,
            12.0,
            usuario,
            new List<PistaReservada>()
            );

            reserva.PistasReservadas.Add(new PistaReservada
            {
                IdReserva = 1,
                IdPista = 1,
                Pista = pista,
                Cantidad = 1,
                Precio = 50,
                Observaciones = "Sin observaciones"
            });

            _context.TipoDeporte.Add(tipo);
            _context.Pistas.Add(pista);
            _context.Reservas.Add(reserva);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReserva_OK_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReservaController>>();
            var controller = new ReservaController(_context, mockLogger.Object);

            // Act
            var actionResult = await controller.GetReserva(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualDTO = Assert.IsType<ReservaDetailDTO>(okResult.Value);

            Assert.Equal(1, actualDTO.Id);
            Assert.Equal("María", actualDTO.NombreUsuario);
            Assert.Equal("García", actualDTO.ApellidosUsuario);
            Assert.Equal(12.0, actualDTO.PrecioTotal);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReserva_NotFound_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReservaController>>();
            var controller = new ReservaController(_context, mockLogger.Object);

            // Act
            var actionResult = await controller.GetReserva(999);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}