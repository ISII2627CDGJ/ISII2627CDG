using System;

namespace AppForSEII.API.DTOs.PistaDTOs
{
    public class PistaDetailDTO : PistaForCreateDTO
    {
         public PistaDetailDTO(int idPista, string nombrePista, string nPersonas, double precio, int stock, int? tipoDeporteId = null, string? nombreTipoDeporte = null)
           : base(nombrePista, nPersonas, precio, stock, tipoDeporteId)
        {
            IdPista = idPista;
            NombreTipoDeporte = nombreTipoDeporte;
        }

        public int IdPista { get; set; }
        public string? NombreTipoDeporte { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PistaDetailDTO dTO &&
                   base.Equals(obj) &&
                   IdPista == dTO.IdPista &&
                   NombreTipoDeporte == dTO.NombreTipoDeporte;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(IdPista);
            hash.Add(NombreTipoDeporte);
            return hash.ToHashCode();
        }
    }
}