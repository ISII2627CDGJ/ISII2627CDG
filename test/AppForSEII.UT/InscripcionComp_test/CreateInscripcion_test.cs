using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppForSEII.UT
{
    public class CreateInscripcion_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime Fecha = new DateTime(2027, 1, 15, 10, 0, 0);

        public CreateInscripcion_test()
        {
            var futbol = new TipoDeporte(1, "Fútbol", "Fútbol", "Deporte de equipo", "11vs11");
            _context.TipoDeporte.Add(futbol);

            // Ids esperados (BD nueva por test): 1, 2 y 3
            _context.Competiciones.AddRange(
                new Competicion("Torneo de fútbol", Fecha, "Albacete", 20, 10m, 1),
                new Competicion("Copa sin plazas", Fecha, "Albacete", 0, 5m, 1),
                new Competicion("Copa de fútbol", Fecha, "Madrid", 15, 20m, 1));
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateInscripcion_Created_test()
        {
            // Arrange
            var controller = new InscripcionesCompController(_context, NullLogger<InscripcionesCompController>.Instance);
            var competicionesPedidas = new List<CompeticionInscritaDTO>
            {
                new CompeticionInscritaDTO(1, "Torneo de fútbol", "Fútbol", Fecha, "Albacete", 10m, "Ninguno"),
                new CompeticionInscritaDTO(3, "Copa de fútbol", "Fútbol", Fecha, "Madrid", 20m, null)
            };
            var peticion = new POSTInscripcionCompDTO("Elena", "Navarro Martínez", "12345678A", "elena@gmail.com", "600000000", "Bizum", "600000000", competicionesPedidas);
            var esperado = new InscripcionCompDetailDTO(1, DateTime.Now, "Elena", "Navarro Martínez",
                "12345678A", "elena@gmail.com", "600000000", competicionesPedidas);

            // Act
            var result = await controller.CreateInscripcion(peticion);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<InscripcionCompDetailDTO>(created.Value);
            Assert.Equal(esperado, actual);
            Assert.Equal(19, _context.Competiciones.Find(1)!.Plazas);
            
            Assert.Equal(1, _context.InscripcionesComp.Count());
            Assert.Equal(2, _context.CompeticionesInscritas.Count());
            Assert.Equal(14, _context.Competiciones.Find(3)!.Plazas);
        }

        public static IEnumerable<object?[]> CasosBadRequest()
        {
            // Carrito vacío
            yield return new object?[] { new List<CompeticionInscritaDTO>() };
            // Competición repetida
            yield return new object?[] { new List<CompeticionInscritaDTO>
            {
                new CompeticionInscritaDTO(1, "Torneo de fútbol", "Fútbol", Fecha, "Albacete", 10m, null),
                new CompeticionInscritaDTO(1, "Torneo de fútbol", "Fútbol", Fecha, "Albacete", 10m, null)
            } };
            // Competición que no existe
            yield return new object?[] { new List<CompeticionInscritaDTO>
            {
                new CompeticionInscritaDTO(99, "No existe", "Fútbol", Fecha, "Albacete", 10m, null)
            } };
            // Competición sin plazas
            yield return new object?[] { new List<CompeticionInscritaDTO>
            {
                new CompeticionInscritaDTO(2, "Copa sin plazas", "Fútbol", Fecha, "Albacete", 5m, null)
            } };
        }

        [Theory]
        [MemberData(nameof(CasosBadRequest))]
        public async Task CreateInscripcion_BadRequest_test(List<CompeticionInscritaDTO> competiciones)
        {
            // Arrange
            var controller = new InscripcionesCompController(_context, NullLogger<InscripcionesCompController>.Instance);
            var peticion = new POSTInscripcionCompDTO("Elena", "Navarro Martínez", "12345678A", "elena@mail.com", "600000000", "Bizum", "600000000", competiciones);

            // Act
            var result = await controller.CreateInscripcion(peticion);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
    
}