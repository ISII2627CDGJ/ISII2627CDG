using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace AppForSEII.API.Models;

public class Alquiler
{
    [Key]
    public int IdAlquiler { get; set; }

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
    public DateTime FechaAlquiler { get; set; }

    [Required]
    [StringLength(50)]
    public string MetodoPago { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string NumeroTelefono { get; set; } = string.Empty;

    [Required]
    [Precision(7, 2)]
    public decimal PrecioTotal { get; set; }
 
    // Relación 1-N con MaterialAlquilado
    public List<MaterialAlquilado> MaterialesAlquilados { get; set; }
 
    //constructores
    public Alquiler()
    {
    }

    public Alquiler(
        int idAlquiler,
        string nombreUsuario,
        string apellidosUsuario,
        string dni,
        DateTime fechaAlquiler,
        string metodoPago,
        string numeroTelefono,
        decimal precioTotal)
    {
        IdAlquiler = idAlquiler;
        NombreUsuario = nombreUsuario;
        ApellidosUsuario = apellidosUsuario;
        DNI = dni;
        FechaAlquiler = fechaAlquiler;
        MetodoPago = metodoPago;
        NumeroTelefono = numeroTelefono;
        PrecioTotal = precioTotal;
        MaterialesAlquilados = new List<MaterialAlquilado>();
    }
    //metodo equals 
    public override bool Equals(object? obj)
    {
        if (obj is Alquiler other)
        {
            return IdAlquiler == other.IdAlquiler &&
                   NombreUsuario == other.NombreUsuario &&
                   ApellidosUsuario == other.ApellidosUsuario &&
                   DNI == other.DNI &&
                   FechaAlquiler == other.FechaAlquiler &&
                   MetodoPago == other.MetodoPago &&
                   NumeroTelefono == other.NumeroTelefono &&
                   PrecioTotal == other.PrecioTotal;
        }

        return false;
    }
}