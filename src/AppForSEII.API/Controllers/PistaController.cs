
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

using AppForSEII.API.Data;

using AppForSEII.API.DTOs.PistaDTOs;

using AppForSEII.API.Models;

using System.Linq;

using System.Threading.Tasks;



namespace AppForSEII.API.Controllers

{

    public class PistasController : ControllerBase

    {

        private readonly ApplicationDbContext _context;

        private readonly ILogger<PistasController> _logger;



        public PistasController(ApplicationDbContext context, ILogger<PistasController> logger)

        {

            _context = context;

            _logger = logger;

        }



        [HttpGet("{id}")]

        public async Task<ActionResult<PistaDetailDTO>> GetPista(int id)

        {

            var pista = await _context.Pistas

                .Include(p => p.TipoDeporte)

                .FirstOrDefaultAsync(p => p.IdPista == id);



            if (pista == null)

            {

                return NotFound();

            }



            var dto = new PistaDetailDTO(
                pista.IdPista,
                pista.NombrePista,
                pista.TipoDeporte.Nombre,
                0.0, 
                0,   
                null,
                null  
            );



            return Ok(dto);

        }

    }

}
