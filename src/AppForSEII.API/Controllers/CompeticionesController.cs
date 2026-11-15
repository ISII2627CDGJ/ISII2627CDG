using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
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
        [ProducesResponseType(typeof(IList<CompeticionParaInscripcionDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCompeticionesParaInscripcion(string? lugar, string? tipoDeporte)
        {
            var consulta = _context.Competiciones
                .Include(c => c.TipoDeporte)
                .Where(c => c.Plazas > 0);

            if (!string.IsNullOrEmpty(lugar))
            {
                consulta = consulta.Where(c => c.Lugar.Contains(lugar));
            }

            if (!string.IsNullOrEmpty(tipoDeporte))
            {
                consulta = consulta.Where(c =>
                    c.TipoDeporte.NombreTipoDeporte == tipoDeporte ||
                    c.TipoDeporte.Nombre == tipoDeporte);
            }

            var competiciones = await consulta
                .OrderBy(c => c.Nombre)
                .Select(c => new CompeticionParaInscripcionDTO(
                    c.Id,
                    c.Nombre,
                    c.TipoDeporte.Nombre,
                    c.Fecha,
                    c.Lugar,
                    c.Precio
                ))
                .ToListAsync();

            if (competiciones.Count == 0)
            {
                return NotFound();
            }

            return Ok(competiciones);
        }
    }
}