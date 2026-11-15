using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.PistaDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII.UT.PistaController_test
{
    public class GetPista_test : AppForSEII4SqliteUT
    {
        public GetPista_test()
        {
            var tiposDeporte = new List<TipoDeporte>()
            {
                new TipoDeporte { Id = 1, Nombre = "Fútbol" },
                new TipoDeporte { Id = 2, Nombre = "Tenis" },
                new TipoDeporte { Id = 3, Nombre = "Pádel" }
            };

            var pistas = new List<Pista>()
            {
                new Pista { IdPista = 1, NombrePista = "Pista Futbol", NPersonas = "20", Precio = 30.0, Stock = 1, TipoDeporte = tiposDeporte[0] },
                new Pista { IdPista = 2, NombrePista = "Pista Tenis 1", NPersonas = "4", Precio = 10.0, Stock = 1, TipoDeporte = tiposDeporte[1] },
                new Pista { IdPista = 3, NombrePista = "Pista Pádel 1", NPersonas = "4", Precio = 20.0, Stock = 1, TipoDeporte = tiposDeporte[2] }
            };

            _context.AddRange(tiposDeporte);
            _context.AddRange(pistas);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetPista_OK()
        {
            var pistaDetailDTOs = new List<PistaDetailDTO>()
            {
                new PistaDetailDTO(1, "Pista Futbol", "20", 30.0, 1, 1, "Fútbol"),
                new PistaDetailDTO(2, "Pista Tenis 1", "4", 10.0, 1, 2, "Tenis"),
                new PistaDetailDTO(3, "Pista Pádel 1", "4", 20.0, 1, 3, "Pádel")
            };

            var allTests = new List<object?[]>
            {
                new object?[] { 1, pistaDetailDTOs[0] },
                new object?[] { 2, pistaDetailDTOs[1] },
                new object?[] { 3, pistaDetailDTOs[2] }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetPista_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPista_OK_test(int idPista, PistaDetailDTO expectedPista)
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var result = await controller.GetPista(idPista);

            var actionResult = Assert.IsType<ActionResult<PistaDetailDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var pistaDTOActual = Assert.IsType<PistaDetailDTO>(okResult.Value);

            Assert.Equal(expectedPista, pistaDTOActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPista_NotFound_test()
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var result = await controller.GetPista(999);

            var actionResult = Assert.IsType<ActionResult<PistaDetailDTO>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}