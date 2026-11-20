using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class CompeticionInscritaDTO
    {
        public int CompeticionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoDeporte { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }

        [StringLength(200)]
        public string? ProblemasFisicos { get; set; }

        public CompeticionInscritaDTO()
        {
        }

        public CompeticionInscritaDTO(int competicionId, string nombre, string nombreTipoDeporte,
            DateTime fecha, string lugar, decimal precio, string? problemasFisicos = null)
        {
            CompeticionId = competicionId;
            Nombre = nombre;
            TipoDeporte = nombreTipoDeporte;
            Fecha = fecha;
            Precio = precio;
            ProblemasFisicos = problemasFisicos;
        }

        public override bool Equals(object? obj)
        {
            return obj is CompeticionInscritaDTO dto &&
                   CompeticionId == dto.CompeticionId &&
                   Nombre == dto.Nombre &&
                   TipoDeporte == dto.TipoDeporte &&
                   Fecha == dto.Fecha &&
                   Precio == dto.Precio &&
                   ProblemasFisicos == dto.ProblemasFisicos;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompeticionId, Nombre, TipoDeporte, Fecha, Precio, ProblemasFisicos);
        }
    }
}