using System.Threading.Tasks;
using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.PistaDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII.UT.PistaController_test
{
    public class GetPista_test
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var dbContext = new ApplicationDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task GetPista_ReturnsOk_WhenPistaExists()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var loggerMock = new Mock<ILogger<PistasController>>();

            var tipoDeporte = new TipoDeporte { Id = 1, Nombre = "Pádel" };
            var pista = new Pista(1, "Pista 1 Pádel", "4", 20.0, 5) { TipoDeporte = tipoDeporte };

            context.TipoDeporte.Add(tipoDeporte);
            context.Pistas.Add(pista);
            await context.SaveChangesAsync();

            var controller = new PistasController(context, loggerMock.Object);

            // Act
            var result = await controller.GetPista(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PistaDetailDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<PistaDetailDTO>(okResult.Value);

            Assert.Equal(1, dto.IdPista);
            Assert.Equal("Pista 1 Pádel", dto.NombrePista);
            Assert.Equal("Pádel", dto.NombreTipoDeporte);
        }

        [Fact]
        public async Task GetPista_ReturnsNotFound_WhenPistaDoesNotExist()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var loggerMock = new Mock<ILogger<PistasController>>();
            var controller = new PistasController(context, loggerMock.Object);

            // Act
            var result = await controller.GetPista(999);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PistaDetailDTO>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}