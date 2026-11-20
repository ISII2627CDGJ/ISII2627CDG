using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.PistaDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.PistaController_test
{
    public class CreatePista_test : AppForSEII4SqliteUT
    {
        public CreatePista_test()
        {
            var tipoDeporte = new TipoDeporte { Id = 1, Nombre = "Fútbol" };

            var pistaExistente = new Pista
            {
                IdPista = 1,
                NombrePista = "Pista Futbol",
                NPersonas = "20",
                Precio = 30.0,
                Stock = 1,
                TipoDeporte = tipoDeporte
            };

            _context.Add(tipoDeporte);
            _context.Add(pistaExistente);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePista_Error()
        {
            var nombreVacio = new PistaForCreateDTO("", "10", 25.0, 2, 1);
            var precioNegativo = new PistaForCreateDTO("Pista Nueva", "10", -5.0, 2, 1);
            var stockNegativo = new PistaForCreateDTO("Pista Nueva", "10", 25.0, -1, 1);
            var deporteNoExiste = new PistaForCreateDTO("Pista Nueva", "10", 25.0, 2, 99);
            var nombreDuplicado = new PistaForCreateDTO("Pista Futbol", "10", 25.0, 2, 1);

            return new List<object[]>
            {
                new object[] { nombreVacio, "¡Error! El nombre de la pista es obligatorio." },
                new object[] { precioNegativo, "¡Error! El precio no puede ser negativo." },
                new object[] { stockNegativo, "¡Error! El stock no puede ser negativo." },
                new object[] { deporteNoExiste, "¡Error! El tipo de deporte con ID '99' no existe." },
                new object[] { nombreDuplicado, "¡Error! Ya existe una pista con el nombre 'Pista Futbol'." }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_CreatePista_Error))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePista_Error_test(PistaForCreateDTO pistaDTO, string errorExpected)
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var result = await controller.CreatePista(pistaDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePista_Success_test()
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var pistaDTO = new PistaForCreateDTO("Pista Central", "14", 35.0, 2, 1);
            var expectedPistaDetail = new PistaDetailDTO(2, "Pista Central", "14", 35.0, 2, 1, "Fútbol");

            var result = await controller.CreatePista(pistaDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPistaDetail = Assert.IsType<PistaDetailDTO>(createdResult.Value);

            Assert.Equal(expectedPistaDetail, actualPistaDetail);
            Assert.Equal(2, _context.Pistas.Count());
        }
    }
}