
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII.API.DTOs
{
    public class MaterialParaAlquilarDTO
    {
        public int IdMaterial { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre del material no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Range(0, 100000, ErrorMessage = "La cantidad debe estar entre 0 y 100000.")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "El precio debe estar entre 0 y 10000.")]
        public double Precio { get; set; }

        [Required]
        public string NombreTipoDeporte { get; set; } = string.Empty;

        [Required]
        public string NombreTipoMaterial { get; set; } = string.Empty;

        // Constructores
        public MaterialParaAlquilarDTO()
        {
        }

        public MaterialParaAlquilarDTO(int idMaterial, string nombre, int cantidad, double precio, string nombreTipoDeporte, string nombreTipoMaterial)
        {
            IdMaterial = idMaterial;
            Nombre = nombre;
            Cantidad = cantidad;
            Precio = precio;
            NombreTipoDeporte = nombreTipoDeporte;
            NombreTipoMaterial = nombreTipoMaterial;
        }

        // Equals & GetHashCode
        public override bool Equals(object? obj)
        {
            return obj is MaterialParaAlquilarDTO dto &&
                   IdMaterial == dto.IdMaterial &&
                   Nombre == dto.Nombre &&
                   Cantidad == dto.Cantidad &&
                   Precio == dto.Precio &&
                   NombreTipoDeporte == dto.NombreTipoDeporte &&
                   NombreTipoMaterial == dto.NombreTipoMaterial;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdMaterial, Nombre, Cantidad, Precio, NombreTipoDeporte, NombreTipoMaterial);
        }
    }
}
