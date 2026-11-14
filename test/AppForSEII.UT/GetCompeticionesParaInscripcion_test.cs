using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppForSEII.UT
{
    public class GetCompeticionesParaInscripcion_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime Fecha = new DateTime(2027, 1, 15, 10, 0, 0);

        public GetCompeticionesParaInscripcion_test()
        {
            // TODO: ajustar los constructores a los de tu proyecto
            var futbol = new TipoDeporte(1,"Fútbol", "Fútbol", "Deporte de equipo","11vs11");
            var tenis = new TipoDeporte(2,"Tenis", "Tenis", "Deporte de raqueta","1vs1");
            _context.TipoDeporte.AddRange(futbol, tenis);

            // Ids esperados (BD nueva por test): 1, 2 y 3
            _context.Competiciones.AddRange(
                new Competicion("Torneo de fútbol", Fecha, "Albacete", 20, 10m, 1),
                new Competicion("Liga de tenis", Fecha, "Madrid", 15, 20m, 2),
                new Competicion("Copa sin plazas", Fecha, "Albacete", 0, 5m, 1));
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> CasosOk()
        {
            var torneo = new CompeticionParaInscripcionDTO(1, "Torneo de fútbol", "Fútbol", Fecha, "Albacete", 10m);
            var liga = new CompeticionParaInscripcionDTO(2, "Liga de tenis", "Tenis", Fecha, "Madrid", 20m);

            // Sin filtros: todas las que tienen plazas, ordenadas por nombre
            yield return new object?[] { null, null, new List<CompeticionParaInscripcionDTO> { liga, torneo } };
            // Por lugar
            yield return new object?[] { "Albacete", null, new List<CompeticionParaInscripcionDTO> { torneo } };
            // Por tipo de deporte
            yield return new object?[] { null, "Tenis", new List<CompeticionParaInscripcionDTO> { liga } };
        }

        [Theory]
        [MemberData(nameof(CasosOk))]
        public async Task GetCompeticionesParaInscripcion_OK_test(
            string? lugar, string? tipoDeporte, List<CompeticionParaInscripcionDTO> esperadas)
        {
            // Arrange
            var controller = new CompeticionesController(_context, NullLogger<CompeticionesController>.Instance);

            // Act
            var result = await controller.GetCompeticionesParaInscripcion(lugar, tipoDeporte);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var actuales = Assert.IsAssignableFrom<IList<CompeticionParaInscripcionDTO>>(ok.Value);
            Assert.True(esperadas.SequenceEqual(actuales));
        }

        [Fact]
        public async Task GetCompeticionesParaInscripcion_NotFound_test()
        {
            // Arrange
            var controller = new CompeticionesController(_context, NullLogger<CompeticionesController>.Instance);

            // Act
            var result = await controller.GetCompeticionesParaInscripcion("Teruel", null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
