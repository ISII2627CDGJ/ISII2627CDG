
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.MaterialDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII.UT.AlquilerController_test
{
    public class PostAlquilarMaterial_test : AppForSEII4SqliteUT
    {
        // Datos comunes para las pruebas
        private const string _email = "elena@uclm.es";
        private const string _metodoPago = "Tarjeta";
        

        // Constructor de la clase de pruebas
        public PostAlquilarMaterial_test()
        {
        }

        // Casos de prueba que deben producir errores
        public static IEnumerable<object[]> TestCasesFor_PostAlquilarMaterial()
        {
            // Caso 1: alquiler sin materiales
            var alquilerSinMateriales = new AlquilerForCreateDTO
            {
                Email=_email,
                FechaAlquiler = DateTime.Today.AddDays(1),
                MetodoPago = _metodoPago,
                MaterialesAlquilados = new List<MaterialAlquiladoItemDTO>()
            };

            // Caso 2: fecha anterior a hoy
            var alquilerFechaIncorrecta = new AlquilerForCreateDTO
            {
                Email=_email,
                FechaAlquiler = DateTime.Today.AddDays(-1),
                MetodoPago = _metodoPago,
                MaterialesAlquilados = new List<MaterialAlquiladoItemDTO>()
            };

                        // Caso 3: el usuario no está registrado
            var alquilerUsuarioNoExiste = new AlquilerForCreateDTO
            {
                Email = "noexiste@uclm.es",
                FechaAlquiler = DateTime.Today.AddDays(1),
                MetodoPago = _metodoPago,
                MaterialesAlquilados = new List<MaterialAlquiladoItemDTO>
                {
                    new MaterialAlquiladoItemDTO(1, "Balon de futbol", "Balones", "Futbol", 1, 5.00m)
                }
            };

            var allTests = new List<object[]>
            {
                new object[]
                {
                    alquilerSinMateriales,
                    "Debe seleccionar al menos un material."
                },
                new object[]
                {
                    alquilerFechaIncorrecta,
                    "La fecha del alquiler no puede ser anterior a hoy."
                },
                new object[]
                {
                    alquilerUsuarioNoExiste,
                    "El usuario con email 'noexiste@uclm.es'"
                }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_PostAlquilarMaterial))]
        public async Task PostAlquilarMaterial_Error_test(
            AlquilerForCreateDTO alquilerDTO,
            string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();

            ILogger<AlquilerController> logger = mock.Object;

            var controller = new AlquilerController(
                _context,
                logger
            );

            // Act
            var result = await controller.PostAlquilarMaterial(
                alquilerDTO
            );

            // Assert
            var badRequestResult =
                Assert.IsType<BadRequestObjectResult>(result);

            var problemDetails =
                Assert.IsType<ValidationProblemDetails>(
                    badRequestResult.Value
                );

            var errores = problemDetails.Errors
                .SelectMany(e => e.Value)
                .Where(e => e != null);

            Assert.Contains(
                errores,
                error => error!.StartsWith(errorExpected)
            );
        }
    }
}
