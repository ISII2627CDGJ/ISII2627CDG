using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace AppForSEII.API.Models;

public class Alquiler
{
    [Key]
    public int IdAlquiler { get; set; }

    [Required]
    public DateTime FechaAlquiler { get; set; }

    [Required]
    [StringLength(50)]
    public string MetodoPago { get; set; } = string.Empty;

    [Required]
    [Precision(7, 2)]
    public decimal PrecioTotal { get; set; }
 
    // Relación 1-N con MaterialAlquilado
    public List<MaterialAlquilado> MaterialesAlquilados { get; set; }

    [Required]
    public ApplicationUser Cliente { get; set; } = null!;
 
    //constructores
    public Alquiler()
    {
    }

    public Alquiler(
        int idAlquiler,
        DateTime fechaAlquiler,
        string metodoPago,
        decimal precioTotal,
        ApplicationUser cliente)
    {
        IdAlquiler = idAlquiler;
        FechaAlquiler = fechaAlquiler;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
        Cliente = cliente;
        MaterialesAlquilados = new List<MaterialAlquilado>();
    }
    //metodo equals 
    public override bool Equals(object? obj)
    {
        return obj is Alquiler other&&
            IdAlquiler == other.IdAlquiler &&
            FechaAlquiler == other.FechaAlquiler &&
            MetodoPago == other.MetodoPago &&
            PrecioTotal == other.PrecioTotal;
        }
    //Metodo HaschCode
    public override int GetHashCode(){
        return HashCode.Combine(IdAlquiler, FechaAlquiler, MetodoPago, PrecioTotal);
    }
}

