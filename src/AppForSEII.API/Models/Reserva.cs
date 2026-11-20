using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Reserva
{
    [Key]
    public int IdReserva { get; set; }

    [Required]
    public DateTime FechaReserva { get; set; } = DateTime.Now;

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    [Range(0, 10000, ErrorMessage = "El precio total debe estar entre 0 y 10000.")]
    public double PrecioTotal { get; set; }

    // Relaciones
    public List<PistaReservada> PistasReservadas { get; set; } = new List<PistaReservada>();

    [Required]
    public ApplicationUser Cliente { get; set; } = null!;

    // Constructores
    public Reserva()
    {
        PistasReservadas = new List<PistaReservada>();
    }

    public Reserva(int idReserva, DateTime fechaReserva, MetodoPago metodoPago, double precioTotal, ApplicationUser cliente, List<PistaReservada> pistasReservadas)
    {
        IdReserva = idReserva;
        FechaReserva = fechaReserva;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
        Cliente = cliente;
        PistasReservadas = pistasReservadas ?? new List<PistaReservada>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Reserva other)
        {
            return IdReserva == other.IdReserva &&
                   FechaReserva == other.FechaReserva &&
                   MetodoPago == other.MetodoPago &&
                   PrecioTotal == other.PrecioTotal &&
                   EqualityComparer<ApplicationUser>.Default.Equals(Cliente, other.Cliente) &&
                   PistasReservadas.SequenceEqual(other.PistasReservadas);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdReserva, FechaReserva, MetodoPago, PrecioTotal, Cliente);
    }
}