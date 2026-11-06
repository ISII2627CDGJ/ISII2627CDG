using System.Net;
using AppForSEII.API.Data;
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
        [ProducesResponseType(typeof(InscripcionComp), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetInscripcion(int id)
        {
            var inscripcion = await _context.InscripcionesComp
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();

            if (inscripcion == null)
            {
                _logger.LogError($"Error: Inscripcion with id {id} does not exist");
                return NotFound();
            }

            return Ok(inscripcion);
        }
    }
}