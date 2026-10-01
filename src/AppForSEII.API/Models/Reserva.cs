namespace AppForSEII.API.Models;


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
    public string MetodoPago { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [Range(0, 10000)]
    public double PrecioTotal { get; set; }   
    
public Reserva(string apellidos, string dni, DateTime fechaReserva, int id, string metodoPago, string nombreCliente, double precioTotal)
{
    Apellidos = apellidos;
    Dni = dni;
    FechaReserva = fechaReserva;
    Id = id;
    MetodoPago = metodoPago;
    NombreCliente = nombreCliente;
    PrecioTotal = precioTotal;
}

    public override bool Equals(object? obj)
    {
        if (obj is Reserva other)
        {
            return Id == other.Id &&
                   Apellidos == other.Apellidos &&
                   Dni == other.Dni &&
                   FechaReserva == other.FechaReserva &&
                   MetodoPago == other.MetodoPago &&
                   NombreCliente == other.NombreCliente &&
                   PrecioTotal == other.PrecioTotal;
        }
        return false;
    }

}