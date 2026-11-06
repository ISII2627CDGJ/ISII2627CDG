using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDetailDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservaController> _logger;

        public ReservaController(ApplicationDbContext context, ILogger<ReservaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ReservaDetailDTO>> GetReserva(int id)
        {
            if (_context.Reservas == null)
            {
                _logger.LogError("Error: La tabla Reservas no existe");
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Where(r => r.Id == id)
                .Include(r => r.PistasReservadas)
                    .ThenInclude(pr => pr.Pista)
                        .ThenInclude(p => p!.TipoDeporte)
                .Select(r => new ReservaDetailDTO(
                    r.Id,
                    r.NombreCliente,
                    r.Apellidos,
                    r.Dni,
                    "", // Email (según disponibilidad en tu DTO)
                    "", // Teléfono (según disponibilidad en tu DTO)
                    r.MetodoPago,
                    r.PrecioTotal,
                    r.PistasReservadas
                        .Select(pr => new ReservaItemDTO(
                            pr.IdPista,
                            pr.Pista!.NombrePista,
                            pr.Pista.TipoDeporte!.Nombre,
                            pr.Pista.NPersonas,
                            (double)pr.Precio,
                            pr.Observaciones))
                        .ToList<ReservaItemDTO>(),
                    "",
                    r.FechaReserva))
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                _logger.LogError($"Error: La reserva con id {id} no existe");
                return NotFound();
            }

            return Ok(reserva);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReserva(ReservaForCreateDTO reservaForCreate)
        {
            // 1. Validaciones de negocio en Fecha y Pistas
            if (reservaForCreate.FechaReserva.Date < DateTime.Today)
            {
                ModelState.AddModelError("FechaReserva", "¡Error! La fecha de su reserva no puede ser anterior a hoy.");
            }

            if (reservaForCreate.Pistas == null || reservaForCreate.Pistas.Count == 0)
            {
                ModelState.AddModelError("Pistas", "¡Error! Debe incluir al menos una pista para realizar la reserva.");
            }
            else if (reservaForCreate.Pistas.GroupBy(p => p.IdPista).Any(g => g.Count() > 1))
            {
                ModelState.AddModelError("Pistas", "¡Error! Hay pistas repetidas en la solicitud de reserva.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // 2. Consulta y verificación de disponibilidad de stock para la fecha elegida
            // Cambia la línea donde obtienes pistaIds por:
            var pistaIds = reservaForCreate.Pistas?.Select(p => p.IdPista).ToList() ?? new List<int>();
            var fecha = reservaForCreate.FechaReserva.Date;
            var fechaSiguiente = fecha.AddDays(1);

            var pistas = await _context.Pistas
                .Where(p => pistaIds.Contains(p.IdPista))
                .Select(p => new
                {
                    p.IdPista,
                    p.NombrePista,
                    NombreTipoDeporte = p.TipoDeporte != null ? p.TipoDeporte.Nombre : "",
                    p.NPersonas,
                    p.Precio,
                    p.Stock,
                    NumberOfReservas = _context.Reservas
                        .Where(r => r.FechaReserva >= fecha && r.FechaReserva < fechaSiguiente)
                        .SelectMany(r => r.PistasReservadas)
                        .Where(pr => pr.IdPista == p.IdPista)
                        .Sum(pr => (int?)pr.Cantidad) ?? 0
                })
                .ToListAsync();

            // 3. Creación de la Entidad Reserva alineada al modelo exacto
            Reserva reserva = new Reserva
            {
                NombreCliente = reservaForCreate.NombreUsuario,
                Apellidos = reservaForCreate.ApellidosUsuario,
                Dni = reservaForCreate.Dni,
                FechaReserva = reservaForCreate.FechaReserva,
                MetodoPago = reservaForCreate.MetodoPago,
                PrecioTotal = 0,
                PistasReservadas = new List<PistaReservada>()
            };

            foreach (var item in reservaForCreate.Pistas)
            {
                var pista = pistas.FirstOrDefault(p => p.IdPista == item.IdPista);

                if (pista == null)
                {
                    ModelState.AddModelError("Pistas", $"¡Error! La pista con ID '{item.IdPista}' no existe.");
                }
                else if (pista.NumberOfReservas + 1 > pista.Stock)
                {
                    ModelState.AddModelError("Pistas", $"¡Error! La pista '{pista.NombrePista}' no está disponible para el {fecha.ToShortDateString()}.");
                }
                else
                {
                    reserva.PistasReservadas.Add(new PistaReservada
                    {
                        IdPista = pista.IdPista,
                        Cantidad = 1,
                        Precio = (decimal)pista.Precio,
                        Observaciones = item.Observaciones
                    });

                    // Rellenar datos en los items DTO para la respuesta final
                    item.NombrePista = pista.NombrePista;
                    item.TipoDeporte = pista.NombreTipoDeporte;
                    item.NPersonas = pista.NPersonas;
                    item.Precio = pista.Precio;
                }
            }

            // Asignar precio total sumado
            reserva.PrecioTotal = (double)reserva.PistasReservadas.Sum(pr => pr.Precio * pr.Cantidad);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // 4. Guardar en la Base de Datos
            _context.Reservas.Add(reserva);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la reserva en la base de datos.");
                return Conflict("Error: " + ex.Message);
            }

            // 5. Generar DTO detallado de respuesta
            var reservaDetail = new ReservaDetailDTO(
                reserva.Id,
                reservaForCreate.NombreUsuario,
                reservaForCreate.ApellidosUsuario,
                reservaForCreate.Dni,
                reservaForCreate.Email,
                reservaForCreate.Telefono,
                reserva.MetodoPago,
                reserva.PrecioTotal,
                reservaForCreate.Pistas,
                reservaForCreate.DatosPago,
                reserva.FechaReserva
            );

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reservaDetail);
        }
    }
}