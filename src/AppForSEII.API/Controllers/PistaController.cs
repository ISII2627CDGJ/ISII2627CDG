using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.PistaDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PistasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PistasController> _logger;

        public PistasController(ApplicationDbContext context, ILogger<PistasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PistaItemDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<PistaItemDTO>>> GetPistas(string? nombrePista = null, string? tipoDeporte = null)
        {
            var pistas = await _context.Pistas
                .Include(p => p.TipoDeporte)
                .Where(p =>
                    (nombrePista == null || p.NombrePista.Contains(nombrePista)) &&
                    (tipoDeporte == null || (p.TipoDeporte != null && p.TipoDeporte.Nombre == tipoDeporte))
                )
                .OrderBy(p => p.NombrePista)
                .Select(p => new PistaItemDTO(
                    p.IdPista,
                    p.NombrePista,
                    p.NPersonas,
                    p.Precio,
                    p.Stock,
                    p.TipoDeporte != null ? p.TipoDeporte.Nombre : null
                ))
                .ToListAsync();

            return Ok(pistas);
        }

      
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PistaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PistaDetailDTO>> GetPista(int id)
        {
            var pista = await _context.Pistas
                .Include(p => p.TipoDeporte)
                .FirstOrDefaultAsync(p => p.IdPista == id);

            if (pista == null)
            {
                return NotFound();
            }

            var pistaDTO = new PistaDetailDTO(
                pista.IdPista,
                pista.NombrePista,
                pista.NPersonas,
                pista.Precio,
                pista.Stock,
                pista.TipoDeporte?.Id,
                pista.TipoDeporte?.Nombre
            );

            return Ok(pistaDTO);
        }

       
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PistaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePista(PistaForCreateDTO pistaForCreate)
        {
            if (string.IsNullOrWhiteSpace(pistaForCreate.NombrePista))
            {
                ModelState.AddModelError("NombrePista", "¡Error! El nombre de la pista es obligatorio.");
            }

            if (pistaForCreate.Precio < 0)
            {
                ModelState.AddModelError("Precio", "¡Error! El precio no puede ser negativo.");
            }

            if (pistaForCreate.Stock < 0)
            {
                ModelState.AddModelError("Stock", "¡Error! El stock no puede ser negativo.");
            }

            TipoDeporte? tipoDeporte = null;
            if (pistaForCreate.TipoDeporteId.HasValue)
            {
                tipoDeporte = await _context.TipoDeporte.FindAsync(pistaForCreate.TipoDeporteId.Value);
                if (tipoDeporte == null)
                {
                    ModelState.AddModelError("TipoDeporteId", $"¡Error! El tipo de deporte con ID '{pistaForCreate.TipoDeporteId.Value}' no existe.");
                }
            }

            if (await _context.Pistas.AnyAsync(p => p.NombrePista == pistaForCreate.NombrePista))
            {
                ModelState.AddModelError("NombrePista", $"¡Error! Ya existe una pista con el nombre '{pistaForCreate.NombrePista}'.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var pista = new Pista
            {
                NombrePista = pistaForCreate.NombrePista,
                NPersonas = pistaForCreate.NPersonas,
                Precio = pistaForCreate.Precio,
                Stock = pistaForCreate.Stock,
                TipoDeporte = tipoDeporte!
            };

            _context.Pistas.Add(pista);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la pista en la base de datos.");
                return Conflict("Error: " + ex.Message);
            }

            var pistaDetail = new PistaDetailDTO(
                pista.IdPista,
                pista.NombrePista,
                pista.NPersonas,
                pista.Precio,
                pista.Stock,
                tipoDeporte?.Id,
                tipoDeporte?.Nombre
            );

            return CreatedAtAction(nameof(GetPista), new { id = pista.IdPista }, pistaDetail);
        }
    }
}