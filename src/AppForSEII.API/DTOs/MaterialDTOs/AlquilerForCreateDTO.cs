
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
            string email,
            DateTime fechaAlquiler,
            string metodoPago,
            List<MaterialAlquiladoItemDTO> materialesAlquilados)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
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
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

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
                   Email == dto.Email &&
                   FechaAlquiler == dto.FechaAlquiler &&
                   MetodoPago == dto.MetodoPago &&
                   MaterialesAlquilados.SequenceEqual(dto.MaterialesAlquilados);
        }

        
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            hash.Add(Email);
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
