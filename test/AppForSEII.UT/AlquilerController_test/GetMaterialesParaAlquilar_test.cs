
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII.UT.AlquilerController_test
{
    public class GetMaterialesParaAlquilar_test : AppForSEII4SqliteUT
    {
        public GetMaterialesParaAlquilar_test()
        {
            var tipoMaterial = new TipoMaterial
            {
                NombreTipoMaterial = "Balones"
            };

            var tipoDeporte = new TipoDeporte
            {
                NombreTipoDeporte = "Futbol"
            };

            var materiales = new List<Material>
            {
                new Material
                {
                    Nombre = "Balon de futbol",
                    Cantidad = 10,
                    Precio = 15,
                    TipoMaterial = tipoMaterial,
                    TipoDeporte = tipoDeporte
                },
                new Material
                {
                    Nombre = "Balon profesional",
                    Cantidad = 5,
                    Precio = 30,
                    TipoMaterial = tipoMaterial,
                    TipoDeporte = tipoDeporte
                }
            };

            _context.Add(tipoMaterial);
            _context.Add(tipoDeporte);
            _context.AddRange(materiales);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaterialesParaAlquilar_OK_test()
        {
            // Arrange
            var mock = new Mock<ILogger<MaterialesController>>();
            var controller = new MaterialesController(
                _context,
                mock.Object
            );

            // Act
            var result = await controller.GetMaterialesParaAlquilar(
                null,
                null
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var materialesActuales =
                Assert.IsAssignableFrom<IList<MaterialParaAlquilarDTO>>(
                    okResult.Value
                );

            Assert.Equal(2, materialesActuales.Count);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaterialesParaAlquilar_TipoMaterial_test()
        {
            // Arrange
            var mock = new Mock<ILogger<MaterialesController>>();
            var controller = new MaterialesController(
                _context,
                mock.Object
            );

            // Act
            var result = await controller.GetMaterialesParaAlquilar(
                "Balones",
                null
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var materialesActuales =
                Assert.IsAssignableFrom<IList<MaterialParaAlquilarDTO>>(
                    okResult.Value
                );

            Assert.Equal(2, materialesActuales.Count);

            Assert.All(materialesActuales, material =>
                Assert.Equal(
                    "Balones",
                    material.NombreTipoMaterial
                )
            );
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaterialesParaAlquilar_PrecioMaximo_test()
        {
            // Arrange
            var mock = new Mock<ILogger<MaterialesController>>();
            var controller = new MaterialesController(
                _context,
                mock.Object
            );

            // Act
            var result = await controller.GetMaterialesParaAlquilar(
                null,
                20
            );

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var materialesActuales =
                Assert.IsAssignableFrom<IList<MaterialParaAlquilarDTO>>(
                    okResult.Value
                );

            Assert.Single(materialesActuales);
            Assert.Equal("Balon de futbol", materialesActuales[0].Nombre);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaterialesParaAlquilar_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<MaterialesController>>();
            var controller = new MaterialesController(
                _context,
                mock.Object
            );

            // Act
            var result = await controller.GetMaterialesParaAlquilar(
                "Material inexistente",
                null
            );

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetMaterialesParaAlquilar_PrecioNegativo_test()
        {
            // Arrange
            var mock = new Mock<ILogger<MaterialesController>>();
            var controller = new MaterialesController(
                _context,
                mock.Object
            );

            // Act
            var result = await controller.GetMaterialesParaAlquilar(
                null,
                -10
            );

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
