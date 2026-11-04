namespace AppForSEII.API.DTOs
{
    public class CompeticionParaInscripcionDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string NombreTipoDeporte { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; } = string.Empty;
        public decimal Precio { get; set; }

        public CompeticionParaInscripcionDTO()
        {
        }

        public CompeticionParaInscripcionDTO(int id, string nombre, string nombreTipoDeporte,
            DateTime fecha, string lugar, decimal precio)
        {
            Id = id;
            Nombre = nombre;
            NombreTipoDeporte = nombreTipoDeporte;
            Fecha = fecha;
            Lugar = lugar;
            Precio = precio;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompeticionParaInscripcionDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   NombreTipoDeporte == dto.NombreTipoDeporte &&
                   Fecha == dto.Fecha &&
                   Lugar == dto.Lugar &&
                   Precio == dto.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, NombreTipoDeporte, Fecha, Lugar, Precio);
        }
    }
}