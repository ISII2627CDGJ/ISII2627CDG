using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppForSEII.UT
{
    public class GetInscripcion_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime Fecha = new DateTime(2027, 1, 15, 10, 0, 0);
        private static readonly DateTime FechaInscripcion = new DateTime(2026, 11, 6, 12, 0, 0);

        public GetInscripcion_test()
        {
            // TODO: ajustar los constructores a los de tu proyecto
            var futbol = new TipoDeporte(1,"Fútbol", "Fútbol", "Deporte de equipo","11vs11");
            var competicion = new Competicion("Torneo de fútbol", Fecha, "Albacete", 20, 10m, 1);
            var inscripcion = new InscripcionComp(FechaInscripcion, 10m, "Bizum",
                "Elena", "Navarro Martínez", "12345678A", "600000000");

            _context.TipoDeporte.Add(futbol);
            _context.Competiciones.Add(competicion);
            _context.InscripcionesComp.Add(inscripcion);
            _context.SaveChanges();   // genera los Ids (1 y 1)

            _context.CompeticionesInscritas.Add(
                new CompeticionInscrita(competicion.Id, inscripcion.Id, "Ninguno"));
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetInscripcion_Found_test()
        {
            // Arrange
            var controller = new InscripcionesCompController(_context, NullLogger<InscripcionesCompController>.Instance);
            var esperado = new InscripcionCompDetailDTO(1, FechaInscripcion, "Elena", "Navarro Martínez",
                "12345678A", "600000000", "Bizum",
                new List<CompeticionInscritaDTO>
                {
                    new CompeticionInscritaDTO(1, "Torneo de fútbol", "Fútbol", Fecha, "Albacete", 10m, "Ninguno")
                });

            // Act
            var result = await controller.GetInscripcion(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<InscripcionCompDetailDTO>(ok.Value);
            Assert.Equal(esperado, actual);
        }

        [Fact]
        public async Task GetInscripcion_NotFound_test()
        {
            // Arrange
            var controller = new InscripcionesCompController(_context, NullLogger<InscripcionesCompController>.Instance);

            // Act
            var result = await controller.GetInscripcion(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
