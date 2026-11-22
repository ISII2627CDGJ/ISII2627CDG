
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.MaterialDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII.UT.AlquilerController_test
{
    public class GetDetailsMaterialAlquilado_test : AppForSEII4SqliteUT
    {
        public GetDetailsMaterialAlquilado_test()
        {
            // Crear el cliente y un alquiler de prueba
            var usuario = new ApplicationUser("1", "Elena", "Navarro", "elena@uclm.es", "12345678Z", 25, "Mujer")
            {
                PhoneNumber = "600123456"
            };
            var alquiler = new Alquiler()
            {
                IdAlquiler = 1,
                FechaAlquiler = new DateTime(2026, 11, 6),
                MetodoPago = "Tarjeta",
                PrecioTotal = 20.00m,
                Cliente=usuario,
                MaterialesAlquilados = new List<MaterialAlquilado>()
            };

            // Guardar el alquiler en SQLite
            _context.ApplicationUsers.Add(usuario);
            _context.Alquileres.Add(alquiler);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetailsMaterialAlquilado_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            // Act
            var result = await controller.GetDetailsMaterialAlquilado(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetailsMaterialAlquilado_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(_context, logger);

            var expectedAlquiler = new AlquilerDetailDTO()
            {
                IdAlquiler = 1,
                NombreUsuario = "Elena",
                ApellidosUsuario = "Navarro",
                DNI = "12345678Z",
                Email="elena@uclm.es",
                NumeroTelefono = "600123456",
                FechaAlquiler = new DateTime(2026, 11, 6),
                MetodoPago = "Tarjeta",
                PrecioTotal = 20.00m,
                MaterialesAlquilados =
                    new List<MaterialAlquiladoItemDTO>()
            };

            // Act
            var result = await controller.GetDetailsMaterialAlquilado(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var alquilerActual =
                Assert.IsType<AlquilerDetailDTO>(okResult.Value);

            Assert.Equal(expectedAlquiler, alquilerActual);
        }
    }
}
