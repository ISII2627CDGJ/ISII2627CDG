
using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaterialesController> _logger;

        public MaterialesController(
            ApplicationDbContext context,
            ILogger<MaterialesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Materiales/GetMaterialesParaAlquilar
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Material>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMaterialesParaAlquilar(string? tipoMaterial, double? precioMaximo)
        {
            // Validar el precio máximo
            if (precioMaximo.HasValue && precioMaximo.Value < 0)
            {
                ModelState.AddModelError(
                    "PrecioMaximo",
                    "El precio máximo no puede ser negativo."
                );

                return BadRequest(
                    new ValidationProblemDetails(ModelState)
                );
            }

            // Obtener los materiales
            var consulta = _context.Materials.AsQueryable();

            // Filtrar por tipo de material
            if (!string.IsNullOrEmpty(tipoMaterial))
            {
                consulta = consulta.Where(m =>  m.TipoMaterial.NombreTipoMaterial == tipoMaterial);
            }

            // Filtrar por precio máximo
            if (precioMaximo.HasValue)
            {
                consulta = consulta.Where(m =>   m.Precio <= precioMaximo.Value);
            }

            // Ejecutar la consulta
            var materiales = await consulta.ToListAsync();

            // Comprobar si existen materiales
            if (materiales.Count == 0)
            {
                return NotFound();
            }

            return Ok(materiales);
        }
    }
}
