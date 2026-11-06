using System;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaItemDTO
    {
        public ReservaItemDTO()
        {
            NombrePista = string.Empty;
            TipoDeporte = string.Empty;
            NPersonas = string.Empty;
        }

        public ReservaItemDTO(int idPista, string nombrePista, string tipoDeporte, string nPersonas, double precio, string? observaciones = null)
        {
            IdPista = idPista;
            NombrePista = nombrePista ?? throw new ArgumentNullException(nameof(nombrePista));
            TipoDeporte = tipoDeporte ?? throw new ArgumentNullException(nameof(tipoDeporte));
            NPersonas = nPersonas ?? throw new ArgumentNullException(nameof(nPersonas));
            Precio = precio;
            Observaciones = observaciones;
        }

        public int IdPista { get; set; }
        public string NombrePista { get; set; }
        public string TipoDeporte { get; set; }
        public string NPersonas { get; set; }
        public double Precio { get; set; }
        public string? Observaciones { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaItemDTO dTO &&
                   IdPista == dTO.IdPista &&
                   NombrePista == dTO.NombrePista &&
                   TipoDeporte == dTO.TipoDeporte &&
                   NPersonas == dTO.NPersonas &&
                   Precio == dTO.Precio &&
                   Observaciones == dTO.Observaciones;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, TipoDeporte, NPersonas, Precio, Observaciones);
        }
    }
}