
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppForSEII.API.DTOs.MaterialDTOs
{
    public class AlquilerForCreateDTO
    {
        // Constructor con parámetros
        public AlquilerForCreateDTO(
            string nombreUsuario,
            string apellidosUsuario,
            string dni,
            string numeroTelefono,
            DateTime fechaAlquiler,
            string metodoPago,
            List<MaterialAlquiladoItemDTO> materialesAlquilados)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
            DNI = dni ?? throw new ArgumentNullException(nameof(dni));
            NumeroTelefono = numeroTelefono ?? throw new ArgumentNullException(nameof(numeroTelefono));
            FechaAlquiler = fechaAlquiler;
            MetodoPago = metodoPago ?? throw new ArgumentNullException(nameof(metodoPago));
            MaterialesAlquilados = materialesAlquilados ?? throw new ArgumentNullException(nameof(materialesAlquilados));
        }

        // Constructor vacío
        public AlquilerForCreateDTO()
        {
            MaterialesAlquilados = new List<MaterialAlquiladoItemDTO>();
        }

        [Required]
        [StringLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ApellidosUsuario { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string DNI { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NumeroTelefono { get; set; } = string.Empty;

        [Required]
        public DateTime FechaAlquiler { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPago { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un material.")]
        public List<MaterialAlquiladoItemDTO> MaterialesAlquilados { get; set; }

       
        public override bool Equals(object? obj)
        {
            return obj is AlquilerForCreateDTO dto &&
                   NombreUsuario == dto.NombreUsuario &&
                   ApellidosUsuario == dto.ApellidosUsuario &&
                   DNI == dto.DNI &&
                   NumeroTelefono == dto.NumeroTelefono &&
                   FechaAlquiler == dto.FechaAlquiler &&
                   MetodoPago == dto.MetodoPago &&
                   MaterialesAlquilados.SequenceEqual(dto.MaterialesAlquilados);
        }

        
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            hash.Add(NombreUsuario);
            hash.Add(ApellidosUsuario);
            hash.Add(DNI);
            hash.Add(NumeroTelefono);
            hash.Add(FechaAlquiler);
            hash.Add(MetodoPago);

            foreach (var material in MaterialesAlquilados)
            {
                hash.Add(material);
            }

            return hash.ToHashCode();
        }
    }
}
