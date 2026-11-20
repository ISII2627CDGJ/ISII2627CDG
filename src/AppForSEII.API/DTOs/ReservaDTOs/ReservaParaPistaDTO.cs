using System;

namespace AppForSEII.API.DTOs.ReservaDetailDTOs
{
    public class ReservaParaPistaDTO
    {
        public ReservaParaPistaDTO()
        {
            NombrePista = string.Empty;
            TipoDeporte = string.Empty;
            NPersonas = string.Empty;
        }

        public ReservaParaPistaDTO(int idPista, string nombrePista, string tipoDeporte, string nPersonas, double precio)
        {
            IdPista = idPista;
            NombrePista = nombrePista;
            TipoDeporte = tipoDeporte;
            NPersonas = nPersonas;
            Precio = precio;
        }

        public int IdPista { get; set; }
        public string NombrePista { get; set; }
        public string TipoDeporte { get; set; }
        public string NPersonas { get; set; }
        public double Precio { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReservaParaPistaDTO dTO &&
                   IdPista == dTO.IdPista &&
                   NombrePista == dTO.NombrePista &&
                   TipoDeporte == dTO.TipoDeporte &&
                   NPersonas == dTO.NPersonas &&
                   Precio == dTO.Precio;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, TipoDeporte, NPersonas, Precio);
        }
    }
}