
using System;

namespace AppForSEII.API.DTOs.MaterialDTOs
{
    public class AlquilerDetailDTO : AlquilerForCreateDTO
    {
        public int IdAlquiler { get; set; }

        public decimal PrecioTotal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetailDTO dto &&
                   base.Equals(obj) &&
                   IdAlquiler == dto.IdAlquiler &&
                   PrecioTotal == dto.PrecioTotal;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(IdAlquiler);
            hash.Add(PrecioTotal);
            return hash.ToHashCode();
        }
    }
}
