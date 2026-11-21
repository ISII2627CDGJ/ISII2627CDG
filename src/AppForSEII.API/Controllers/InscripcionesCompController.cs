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
    public class InscripcionesCompController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InscripcionesCompController> _logger;

        public InscripcionesCompController(
            ApplicationDbContext context,
            ILogger<InscripcionesCompController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: InscripcionesComp/GetInscripcion
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionCompDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetInscripcion(int id)
        {
            var inscripcion = await _context.InscripcionesComp
                .Include(i => i.CompeticionesInscritas)
                    .ThenInclude(ci => ci.Competicion)
                        .ThenInclude(c => c.TipoDeporte)
                .Where(i => i.Id == id)
                .Select(i => new InscripcionCompDetailDTO(
                    i.Id,
                    i.FechaInscripcion,
                    i.Cliente.Name ?? string.Empty,
                    i.Cliente.Surname ?? string.Empty,
                    i.Cliente.DNI ?? string.Empty,
                    i.Cliente.Email ?? string.Empty,
                    i.Cliente.PhoneNumber ?? string.Empty,
                    i.CompeticionesInscritas.Select(ci => new CompeticionInscritaDTO(
                        ci.CompeticionId,
                        ci.Competicion.Nombre,
                        ci.Competicion.TipoDeporte.Nombre,
                        ci.Competicion.Fecha,
                        ci.Competicion.Lugar,
                        ci.Competicion.Precio,
                        ci.ProblemasFisicos
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (inscripcion == null)
            {
                _logger.LogError($"Error: Inscripcion with id {id} does not exist");
                return NotFound();
            }

            return Ok(inscripcion);
        }
                // POST: InscripcionesComp/CreateInscripcion
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(InscripcionCompDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateInscripcion(POSTInscripcionCompDTO inscripcionForCreate)
        {
            if (inscripcionForCreate.Competiciones.Count == 0)
            {
                ModelState.AddModelError("Competiciones", "Debe seleccionar al menos una competición.");
            }
            else if (inscripcionForCreate.Competiciones.GroupBy(c => c.CompeticionId).Any(g => g.Count() > 1))
            {
                ModelState.AddModelError("Competiciones", "Hay competiciones repetidas.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
                        var cliente = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.UserName == inscripcionForCreate.Email || u.Email == inscripcionForCreate.Email);

            if (cliente == null)
            {
                ModelState.AddModelError("Email", $"¡Error! El usuario con email '{inscripcionForCreate.Email}' no está registrado.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            var ids = inscripcionForCreate.Competiciones.Select(c => c.CompeticionId).ToList();
            var competiciones = await _context.Competiciones
                .Include(c => c.TipoDeporte)
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            var inscripcion = new InscripcionComp(
                DateTime.Now,
                0m,
                inscripcionForCreate.MetodoPago,
                cliente);

            var competicionesDTO = new List<CompeticionInscritaDTO>();
            decimal precioTotal = 0m;

            foreach (var item in inscripcionForCreate.Competiciones)
            {
                var competicion = competiciones.FirstOrDefault(c => c.Id == item.CompeticionId);

                if (competicion == null)
                {
                    ModelState.AddModelError("Competiciones", $"La competición con ID {item.CompeticionId} no existe.");
                }
                else if (competicion.Plazas <= 0)
                {
                    ModelState.AddModelError("Competiciones", $"La competición '{competicion.Nombre}' no tiene plazas disponibles.");
                }
                else
                {
                    competicion.Plazas--;
                    precioTotal += competicion.Precio;
                    inscripcion.CompeticionesInscritas.Add(
                        new CompeticionInscrita(competicion.Id, 0, item.ProblemasFisicos));
                    competicionesDTO.Add(new CompeticionInscritaDTO(
                        competicion.Id,
                        competicion.Nombre,
                        competicion.TipoDeporte.Nombre,
                        competicion.Fecha,
                        competicion.Lugar,
                        competicion.Precio,
                        item.ProblemasFisicos));
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            inscripcion.PrecioTotal = precioTotal;
            _context.InscripcionesComp.Add(inscripcion);
            await _context.SaveChangesAsync();

            var inscripcionDetail = new InscripcionCompDetailDTO(
                inscripcion.Id,
                inscripcion.FechaInscripcion,
                cliente.Name ?? string.Empty,
                cliente.Surname ?? string.Empty,
                cliente.DNI ?? string.Empty,
                cliente.Email ?? string.Empty,
                cliente.PhoneNumber ?? string.Empty,
                competicionesDTO);

            return CreatedAtAction(nameof(GetInscripcion), new { id = inscripcion.Id }, inscripcionDetail);
        }
    }
}