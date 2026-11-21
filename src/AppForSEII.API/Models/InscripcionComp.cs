namespace AppForSEII.API.Models;

public class InscripcionComp
{
    public int Id { get; set; }
    [Required]
    public DateTime FechaInscripcion { get; set; } = DateTime.Now;

    [Precision(7, 2)]
    public decimal PrecioTotal { get; set; }

    [Required]
    public string MetodoPago { get; set; }

    [Required]
    [StringLength(50)]
        // Relación N-1 con el cliente: los datos personales se leen de ApplicationUser
    public ApplicationUser Cliente { get; set; } = null!;
    public InscripcionComp(DateTime fechaInscripcion, decimal precioTotal, string metodoPago, ApplicationUser cliente){
        FechaInscripcion = fechaInscripcion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        Cliente = cliente;
    }
    public IList<CompeticionInscrita> CompeticionesInscritas { get; set; } = new List<CompeticionInscrita>();
    public InscripcionComp()
    {
    }
    public override bool Equals(object? obj)
    {
        if (obj is InscripcionComp other)
        {
            return obj is InscripcionComp &&
                   Id == other.Id &&
                   FechaInscripcion == other.FechaInscripcion &&
                   PrecioTotal == other.PrecioTotal &&
                   MetodoPago == other.MetodoPago;
        }
        return false;
        
    }
    public override int GetHashCode(){
        return HashCode.Combine(Id, FechaInscripcion, PrecioTotal, MetodoPago);
    }
}