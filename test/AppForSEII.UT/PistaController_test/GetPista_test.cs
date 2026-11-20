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
    public class GetPistas_test : AppForSEII4SqliteUT
    {
        public GetPistas_test()
        {
            var tiposDeporte = new List<TipoDeporte>()
            {
                new TipoDeporte { Id = 1, Nombre = "Fútbol" },
                new TipoDeporte { Id = 2, Nombre = "Tenis" },
                new TipoDeporte { Id = 3, Nombre = "Pádel" }
            };

            var usuario = new ApplicationUser(
                "1",
                "Elena",
                "Navarro Martínez",
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

            var pistas = new List<Pista>()
            {
                new Pista { IdPista = 1, NombrePista = "Pista Futbol", NPersonas = "20", Precio = 30.0, Stock = 1, TipoDeporte = tiposDeporte[0] },
                new Pista { IdPista = 2, NombrePista = "Pista Tenis 1", NPersonas = "4", Precio = 10.0, Stock = 2, TipoDeporte = tiposDeporte[1] },
                new Pista { IdPista = 3, NombrePista = "Pista Pádel 1", NPersonas = "4", Precio = 20.0, Stock = 3, TipoDeporte = tiposDeporte[2] }
            };

            _context.AddRange(tiposDeporte);
            _context.AddRange(pistas);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetPistas_OK()
        {
            var dto1 = new PistaItemDTO(1, "Pista Futbol", "20", 30.0, 1, "Fútbol");
            var dto2 = new PistaItemDTO(2, "Pista Tenis 1", "4", 10.0, 2, "Tenis");
            var dto3 = new PistaItemDTO(3, "Pista Pádel 1", "4", 20.0, 3, "Pádel");

            return new List<object?[]>
            {
                // Sin filtros (ordenadas alfabéticamente por NombrePista)
                new object?[] { null, null, new List<PistaItemDTO> { dto1, dto3, dto2 } },
                // Filtro por nombrePista
                new object?[] { "Tenis", null, new List<PistaItemDTO> { dto2 } },
                // Filtro por tipoDeporte
                new object?[] { null, "Pádel", new List<PistaItemDTO> { dto3 } },
                // Filtro combinado sin resultados
                new object?[] { "Futbol", "Tenis", new List<PistaItemDTO>() }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetPistas_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPistas_OK_test(string? nombrePista, string? tipoDeporte, IList<PistaItemDTO> expectedPistas)
        {
            var mockLogger = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(_context, mockLogger.Object);

            var result = await controller.GetPistas(nombrePista, tipoDeporte);

            var actionResult = Assert.IsType<ActionResult<IList<PistaItemDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualPistas = Assert.IsAssignableFrom<IList<PistaItemDTO>>(okResult.Value);

            Assert.Equal(expectedPistas, actualPistas);
        }
    }
}