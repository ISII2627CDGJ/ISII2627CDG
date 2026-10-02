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
    public string NombreUsuario { get; set; }

    [Required]
    [StringLength(100)]
    public string ApellidosUsuario { get; set; }

    [Required]
    [StringLength(9)]
    public string DNI { get; set; }

    [Required]
    [Phone]
    public string Telefono { get; set; }
    public InscripcionComp(DateTime fechaInscripcion, decimal precioTotal, string metodoPago, string nombreUsuario, string apellidosUsuario, string dni, string telefono)
    {
        FechaInscripcion = fechaInscripcion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        NombreUsuario = nombreUsuario;
        ApellidosUsuario = apellidosUsuario;
        DNI = dni;
        Telefono = telefono;
    }
    public IList<CompeticionInscrita> CompeticionesInscritas { get; set; } = new List<CompeticionInscrita>();
    public InscripcionComp()
    {
    }
    public override bool Equals(object? obj)
    {
        if (obj is InscripcionComp other)
        {
            return Id == other.Id &&
                   FechaInscripcion == other.FechaInscripcion &&
                   PrecioTotal == other.PrecioTotal &&
                   MetodoPago == other.MetodoPago &&
                   NombreUsuario == other.NombreUsuario &&
                   ApellidosUsuario == other.ApellidosUsuario &&
                   DNI == other.DNI &&
                   Telefono == other.Telefono;
        }
        return false;
    }
}