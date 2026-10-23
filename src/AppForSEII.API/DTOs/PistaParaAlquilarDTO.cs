using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII.API.Models
{
    public class PistaParaAlquilarDTO
    {
        public PistaParaAlquilarDTO()
        {
        }

      public PistaParaAlquilarDTO(int id, string NombrePista,double NPersonas,double Precio, DateTime availableFrom)
        {
            Id = id;
            NombrePista = NombrePista;
            NPersonas = NPersonas;
            Precio = Precio;
            AvailableFrom = availableFrom;
            
        }
        public PistaParaAlquilarDTO(int id, string nombrePista, double nPersonas, double precio, DateTime? lastRental)
            : this(id, nombrePista, nPersonas, precio, lastRental ?? DateTime.MinValue)
        {
            LastRental = lastRental;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name must have a maximum length of 50 characters")]
        public string NombrePista { get; set; } = string.Empty; // Added default value to avoid nullability issues

        [Range(1, 100, ErrorMessage = "Number of people must be between 1 and 100")]
        public double NPersonas { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "Minimum price is 1")]
        [System.ComponentModel.DataAnnotations.Display(Name = "Price For Renting")]
        public double Precio { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [System.ComponentModel.DataAnnotations.Display(Name = "Available From")]
        public DateTime AvailableFrom { get; set; }

        public DateTime? LastRental { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PistaParaAlquilarDTO dto &&
                   Id == dto.Id &&
                   NombrePista == dto.NombrePista &&
                   NPersonas == dto.NPersonas &&
                   Precio == dto.Precio &&
                   AvailableFrom == dto.AvailableFrom &&
                   LastRental == dto.LastRental;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, NombrePista, NPersonas, Precio, AvailableFrom, LastRental);
        }
    }
}