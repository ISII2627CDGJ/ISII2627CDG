namespace AppForSEII.API.Models;


public enum MetodoPago
{
        Bizum,
    Efectivo,
    Tarjeta,
}
public class Reserva
{
    [Required]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    public string Dni { get; set; } = string.Empty;

    [Required]

    public DateTime FechaReserva { get; set; } = DateTime.Now;

    [Key]
    public int Id { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [Range(0, 10000)]
    public double PrecioTotal { get; set; }   
}