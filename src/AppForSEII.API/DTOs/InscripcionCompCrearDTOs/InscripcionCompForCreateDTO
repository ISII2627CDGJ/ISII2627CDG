using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class InscripcionCompForCreateDTO
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
        [Phone]
        public string Telefono { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string MetodoPago { get; set; } = string.Empty;

        public IList<CompeticionInscritaDTO> Competiciones { get; set; } = new List<CompeticionInscritaDTO>();

        public InscripcionCompForCreateDTO()
        {
        }

        public InscripcionCompForCreateDTO(string nombreUsuario, string apellidosUsuario, string dni,
            string telefono, string metodoPago, IList<CompeticionInscritaDTO> competiciones)
        {
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DNI = dni;
            Telefono = telefono;
            MetodoPago = metodoPago;
            Competiciones = competiciones ?? throw new ArgumentNullException(nameof(competiciones));
        }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionCompForCreateDTO dto &&
                   NombreUsuario == dto.NombreUsuario &&
                   ApellidosUsuario == dto.ApellidosUsuario &&
                   DNI == dto.DNI &&
                   Telefono == dto.Telefono &&
                   MetodoPago == dto.MetodoPago &&
                   Competiciones.SequenceEqual(dto.Competiciones);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, ApellidosUsuario, DNI, Telefono, MetodoPago);
        }
    }
}