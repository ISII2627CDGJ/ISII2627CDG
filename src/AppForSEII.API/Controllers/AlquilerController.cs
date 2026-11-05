
using System.Net;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.MaterialDTOs;
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

        //GetDetailsMaterialAlquilado
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDetailsMaterialAlquilado(int id)
        {
            // Obtener el alquiler correspondiente
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

                    // Obtener los materiales asociados al alquiler
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

            // Comprobar si existe el alquiler
            if (alquiler == null)
            {
                _logger.LogWarning(
                    "No se ha encontrado el alquiler con ID {IdAlquiler}",
                    id
                );

                return NotFound();
            }

            // Devolver los detalles del alquiler
            return Ok(alquiler);
        }
    }
}
