
using System;

namespace AppForSEII.API.DTOs.MaterialDTOs
{
    public class MaterialAlquiladoItemDTO
    {
        public MaterialAlquiladoItemDTO(
            int idMaterial,
            string nombreMaterial,
            string tipoMaterial,
            string tipoDeporte,
            int cantidad,
            decimal precio,
            string? descripcion = null)
        {
            IdMaterial = idMaterial;
            NombreMaterial = nombreMaterial ?? throw new ArgumentNullException(nameof(nombreMaterial));
            TipoMaterial = tipoMaterial ?? throw new ArgumentNullException(nameof(tipoMaterial));
            TipoDeporte = tipoDeporte ?? throw new ArgumentNullException(nameof(tipoDeporte));
            Cantidad = cantidad;
            Precio = precio;
            Descripcion = descripcion;
        }

        public int IdMaterial { get; set; }

        public string NombreMaterial { get; set; }

        public string TipoMaterial { get; set; }

        public string TipoDeporte { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public string? Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is MaterialAlquiladoItemDTO dto &&
                   IdMaterial == dto.IdMaterial &&
                   NombreMaterial == dto.NombreMaterial &&
                   TipoMaterial == dto.TipoMaterial &&
                   TipoDeporte == dto.TipoDeporte &&
                   Cantidad == dto.Cantidad &&
                   Precio == dto.Precio &&
                   Descripcion == dto.Descripcion;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(IdMaterial);
            hash.Add(NombreMaterial);
            hash.Add(TipoMaterial);
            hash.Add(TipoDeporte);
            hash.Add(Cantidad);
            hash.Add(Precio);
            hash.Add(Descripcion);
            return hash.ToHashCode();
        }
    }
}
