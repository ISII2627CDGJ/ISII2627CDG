using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompeticionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompeticionesController> _logger;

        public CompeticionesController(
            ApplicationDbContext context,
            ILogger<CompeticionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Competiciones/GetCompeticionesParaInscripcion
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Competicion>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCompeticionesParaInscripcion(string? lugar, string? tipoDeporte)
        {
            // Solo competiciones con plazas libres
            var consulta = _context.Competiciones
                .Where(c => c.Plazas > 0);

            // Filtrar por lugar
            if (!string.IsNullOrEmpty(lugar))
            {
                consulta = consulta.Where(c => c.Lugar.Contains(lugar));
            }

            // Filtrar por tipo de deporte
            if (!string.IsNullOrEmpty(tipoDeporte))
            {
                consulta = consulta.Where(c =>
                    c.TipoDeporte.NombreTipoDeporte == tipoDeporte);
            }

            // Ejecutar la consulta
            var competiciones = await consulta
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            // Comprobar si existen competiciones
            if (competiciones.Count == 0)
            {
                return NotFound();
            }

            return Ok(competiciones);
        }
    }
}