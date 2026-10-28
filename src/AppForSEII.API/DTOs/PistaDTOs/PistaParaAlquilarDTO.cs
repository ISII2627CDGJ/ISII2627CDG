using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII.API.DTOs
{
    public class PistaParaAlquilarDTO
    {
        public int IdPista { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El nombre de la pista no puede exceder los 100 caracteres.")]
        public string NombrePista { get; set; } = string.Empty;

        [Required]
        public string NPersonas { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000, ErrorMessage = "El precio debe estar entre 0 y 10000.")]
        public double Precio { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "El stock debe estar entre 0 y 100000.")]
        public int Stock { get; set; }

        [Required]
        public string NombreTipoDeporte { get; set; } = string.Empty;

        // Constructores
        public PistaParaAlquilarDTO()
        {
        }

        public PistaParaAlquilarDTO(int idPista, string nombrePista, string nPersonas, double precio, int stock, string nombreTipoDeporte)
        {
            IdPista = idPista;
            NombrePista = nombrePista;
            NPersonas = nPersonas;
            Precio = precio;
            Stock = stock;
            NombreTipoDeporte = nombreTipoDeporte;
        }

        // Equals & GetHashCode
        public override bool Equals(object? obj)
        {
            return obj is PistaParaAlquilarDTO dto &&
                   IdPista == dto.IdPista &&
                   NombrePista == dto.NombrePista &&
                   NPersonas == dto.NPersonas &&
                   Precio == dto.Precio &&
                   Stock == dto.Stock &&
                   NombreTipoDeporte == dto.NombreTipoDeporte;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPista, NombrePista, NPersonas, Precio, Stock, NombreTipoDeporte);
        }
    }
}