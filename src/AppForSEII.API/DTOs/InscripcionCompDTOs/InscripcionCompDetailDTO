namespace AppForSEII.API.DTOs
{
    public class InscripcionCompDetailDTO
    {
        public int Id { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string ApellidosUsuario { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public IList<CompeticionInscritaDTO> Competiciones { get; set; } = new List<CompeticionInscritaDTO>();

        public decimal PrecioTotal
        {
            get { return Competiciones.Sum(c => c.Precio); }
        }

        public InscripcionCompDetailDTO()
        {
        }

        public InscripcionCompDetailDTO(int id, DateTime fechaInscripcion, string nombreUsuario,
            string apellidosUsuario, string dni, string telefono, string metodoPago,
            IList<CompeticionInscritaDTO> competiciones)
        {
            Id = id;
            FechaInscripcion = fechaInscripcion;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DNI = dni;
            Telefono = telefono;
            MetodoPago = metodoPago;
            Competiciones = competiciones ?? throw new ArgumentNullException(nameof(competiciones));
        }

        // La fecha de inscripción la genera el servidor: se compara con un minuto de tolerancia
        private static bool CompareDate(DateTime date1, DateTime date2)
        {
            return Math.Abs((date1 - date2).TotalMinutes) < 1;
        }

        public override bool Equals(object? obj)
        {
            return obj is InscripcionCompDetailDTO dto &&
                   Id == dto.Id &&
                   CompareDate(FechaInscripcion, dto.FechaInscripcion) &&
                   NombreUsuario == dto.NombreUsuario &&
                   ApellidosUsuario == dto.ApellidosUsuario &&
                   DNI == dto.DNI &&
                   Telefono == dto.Telefono &&
                   MetodoPago == dto.MetodoPago &&
                   Competiciones.SequenceEqual(dto.Competiciones) &&
                   PrecioTotal == dto.PrecioTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NombreUsuario, ApellidosUsuario, DNI, Telefono, MetodoPago);
        }
    }
}