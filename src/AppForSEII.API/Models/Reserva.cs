namespace AppForSEII.API.Models;


public enum MetodoPago
{
        Bizum,
    Efectivo,
    Tarjeta,
}
public class Reserva
{

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    public string Dni { get; set; } = string.Empty;

    [Required]

    public DateTime FechaReserva { get; set; } = DateTime.Now;

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [Range(0, 10000)]
    public double PrecioTotal { get; set; }
    /*
    relacion 1..N siendo PistaReservada la 
    clase dependiente y la clase Pista principal    
    */
    public List<PistaReservada> PistasReservadas { get; set; }


    //Constructores

    public Reserva()
    {
        PistasReservadas = new List<PistaReservada>();
    }

    public Reserva(int id, string apellidos, string dni, DateTime fechaReserva, MetodoPago metodoPago, string nombreCliente, double precioTotal, List<PistaReservada> pistasReservadas)
    {
        Id = id;
        Apellidos = apellidos;
        Dni = dni;
        FechaReserva = fechaReserva;
        MetodoPago = metodoPago;
        NombreCliente = nombreCliente;
        PrecioTotal = precioTotal;
        PistasReservadas = pistasReservadas ?? new List<PistaReservada>();
    }

//Metodo equals
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
                   PrecioTotal == other.PrecioTotal &&
                   PistasReservadas.SequenceEqual(other.PistasReservadas);
        }
        return false;
    }
}