using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.PistaDTOs
{
    public class PistaForCreateDTO
    {
        public PistaForCreateDTO(string nombrePista, string nPersonas, double precio, int stock, int? tipoDeporteId = null)
        {
            NombrePista = nombrePista ?? throw new ArgumentNullException(nameof(nombrePista));
            NPersonas = nPersonas ?? throw new ArgumentNullException(nameof(nPersonas));
            Precio = precio;
            Stock = stock;
            TipoDeporteId = tipoDeporteId;
        }

        [Required(ErrorMessage = "El nombre de la pista es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre de la pista no puede exceder los 100 caracteres.")]
        public string NombrePista { get; set; }

        [Required(ErrorMessage = "El número de personas es obligatorio.")]
        public string NPersonas { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "El precio debe estar entre 0 y 10000.")]
        public double Precio { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "El stock debe estar entre 0 y 100000.")]
        public int Stock { get; set; }

        public int? TipoDeporteId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PistaForCreateDTO dTO &&
                   NombrePista == dTO.NombrePista &&
                   NPersonas == dTO.NPersonas &&
                   Precio == dTO.Precio &&
                   Stock == dTO.Stock &&
                   TipoDeporteId == dTO.TipoDeporteId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombrePista, NPersonas, Precio, Stock, TipoDeporteId);
        }
    }
}