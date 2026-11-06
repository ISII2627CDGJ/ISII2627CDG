
using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.MaterialDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(
            ApplicationDbContext context,
            ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET DETAILS
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetailsMaterialAlquilado(int id)
        {
            var alquiler = await _context.Alquileres
                .AsNoTracking()
                .Where(a => a.IdAlquiler == id)
                .Select(a => new AlquilerDetailDTO
                {
                    IdAlquiler = a.IdAlquiler,
                    NombreUsuario = a.NombreUsuario,
                    ApellidosUsuario = a.ApellidosUsuario,
                    DNI = a.DNI,
                    NumeroTelefono = a.NumeroTelefono,
                    FechaAlquiler = a.FechaAlquiler,
                    MetodoPago = a.MetodoPago,
                    PrecioTotal = a.PrecioTotal,

                    MaterialesAlquilados = a.MaterialesAlquilados
                        .Select(ma => new MaterialAlquiladoItemDTO(
                            ma.IdMaterial,
                            ma.Material.Nombre,
                            ma.Material.TipoMaterial.NombreTipoMaterial,
                            ma.Material.TipoDeporte.NombreTipoDeporte,
                            ma.Cantidad,
                            ma.Precio,
                            ma.Descripcion
                        ))
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (alquiler == null)
            {
                _logger.LogWarning(
                    "No se ha encontrado el alquiler con ID {IdAlquiler}",
                    id);

                return NotFound();
            }

            return Ok(alquiler);
        }

        // POST ALQUILAR MATERIAL
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> PostAlquilarMaterial(
            [FromBody] AlquilerForCreateDTO alquilerForCreate)
        {
            //  Validar los datos del alquiler
            if (alquilerForCreate.FechaAlquiler.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    "FechaAlquiler",
                    "La fecha del alquiler no puede ser anterior a hoy.");
            }

            if (alquilerForCreate.MaterialesAlquilados == null ||
                alquilerForCreate.MaterialesAlquilados.Count == 0)
            {
                ModelState.AddModelError(
                    "MaterialesAlquilados",
                    "Debe seleccionar al menos un material.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ValidationProblemDetails(ModelState));
            }

            var items = alquilerForCreate.MaterialesAlquilados;

            //  Comprobar las cantidades
            foreach (var item in items)
            {
                if (item.Cantidad <= 0)
                {
                    ModelState.AddModelError(
                        "MaterialesAlquilados",
                        "La cantidad debe ser mayor que cero.");
                }
            }

            // Evitar materiales repetidos
            if (items.GroupBy(m => m.IdMaterial)
                     .Any(g => g.Count() > 1))
            {
                ModelState.AddModelError(
                    "MaterialesAlquilados",
                    "No se puede seleccionar el mismo material varias veces.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ValidationProblemDetails(ModelState));
            }

            //  Consultar los materiales existentes
            var idsMateriales = items
                .Select(m => m.IdMaterial)
                .ToList();

            var materiales = await _context.Materials
                .Where(m => idsMateriales.Contains(m.IdMaterial))
                .ToListAsync();

            foreach (var item in items)
            {
                var material = materiales.FirstOrDefault(
                    m => m.IdMaterial == item.IdMaterial);

                if (material == null)
                {
                    ModelState.AddModelError(
                        "MaterialesAlquilados",
                        $"El material con ID {item.IdMaterial} no existe.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ValidationProblemDetails(ModelState));
            }

            // 4. Crear el alquiler
            var alquiler = new Alquiler
            {
                NombreUsuario = alquilerForCreate.NombreUsuario,
                ApellidosUsuario = alquilerForCreate.ApellidosUsuario,
                DNI = alquilerForCreate.DNI,
                NumeroTelefono = alquilerForCreate.NumeroTelefono,
                FechaAlquiler = alquilerForCreate.FechaAlquiler,
                MetodoPago = alquilerForCreate.MetodoPago,
                PrecioTotal = 0,
                MaterialesAlquilados = new List<MaterialAlquilado>()
            };

            //  Añadir los materiales alquilados
            foreach (var item in items)
            {
                var material = materiales.First(
                    m => m.IdMaterial == item.IdMaterial);

                var materialAlquilado = new MaterialAlquilado
                {
                    Material = material,
                    Alquiler = alquiler,
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion,
                    Precio = item.Precio
                };

                alquiler.MaterialesAlquilados.Add(materialAlquilado);

                alquiler.PrecioTotal += item.Precio * item.Cantidad;
            }

            //  Guardar en la base de datos
            _context.Alquileres.Add(alquiler);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Error al guardar el alquiler de material.");

                return Conflict(
                    "Se ha producido un error al guardar el alquiler.");
            }

            //  Devolver el alquiler creado
            return CreatedAtAction(
                nameof(GetDetailsMaterialAlquilado),
                new { id = alquiler.IdAlquiler },
                new { alquiler.IdAlquiler });
        }
    }
}
