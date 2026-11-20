using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class POSTInscripcionCompDTO
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string ApellidosUsuario { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(9)]
        public string DNI { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]                     
        [EmailAddress]
        public string Email { get; set; } = string.Empty;


        [Required(AllowEmptyStrings = false)]
        [Phone]
        public string Telefono { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string DatosPago { get; set; } = string.Empty;

        public IList<CompeticionInscritaDTO> Competiciones { get; set; } = new List<CompeticionInscritaDTO>();

        public POSTInscripcionCompDTO()
        {
        }

        public POSTInscripcionCompDTO(string nombreUsuario, string apellidosUsuario, string dni, string email,
            string telefono, string metodoPago, string datosPago, IList<CompeticionInscritaDTO> competiciones)
        {
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DNI = dni;
            Email = email; 
            Telefono = telefono;
            MetodoPago = metodoPago;
            DatosPago = datosPago;
            Competiciones = competiciones ?? throw new ArgumentNullException(nameof(competiciones));
        }

        public override bool Equals(object? obj)
        {
            return obj is POSTInscripcionCompDTO dto &&
                   NombreUsuario == dto.NombreUsuario &&
                   ApellidosUsuario == dto.ApellidosUsuario &&
                   DNI == dto.DNI &&
                   Email == dto.Email && 
                   Telefono == dto.Telefono &&
                   MetodoPago == dto.MetodoPago &&
                   DatosPago == dto.DatosPago &&
                   Competiciones.SequenceEqual(dto.Competiciones);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, ApellidosUsuario, DNI, Email, Telefono, MetodoPago, DatosPago);
        }
    }
}