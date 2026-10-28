using System;

namespace AppForSEII.API.DTOs.PistaDTOs
{
    public class PistaItemDTO
    {
        public PistaItemDTO(int idPista, string nombrePista, string nPersonas, double precio, int stock, string? tipoDeporte = null)
        {
            IdPista = idPista;
            NombrePista = nombrePista ?? throw new ArgumentNullException(nameof(nombrePista));
            NPersonas = nPersonas ?? throw new ArgumentNullException(nameof(nPersonas));
            Precio = precio;
            Stock = stock;
            TipoDeporte = tipoDeporte;
        }

        public int IdPista { get; set; }
        public string NombrePista { get; set; }
        public string NPersonas { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public string? TipoDeporte { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PistaItemDTO dTO &&
                   IdPista == dTO.IdPista &&
                   NombrePista == dTO.NombrePista &&
                   NPersonas == dTO.NPersonas &&
                   Precio == dTO.Precio &&
                   Stock == dTO.Stock &&
                   TipoDeporte == dTO.TipoDeporte;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, NPersonas, Precio, Stock, TipoDeporte);
        }
    }
}