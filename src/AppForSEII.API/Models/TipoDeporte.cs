namespace AppForSEII.API.Models;


public class TipoDeporte
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Competiciones { get; set; } = string.Empty;

    // Relación 1-N con Material
    public List<Material> Materiales { get; set; }

    [Required]
    public string Nombre { get; set; }
    [Required]
    public string NombreTipoDeporte { get; set; }

    [Required]
    public string Descripcion { get; set; } = string.Empty;



    //Relacion 1--N con pista
    public List<Pista> Pistas { get; set; }

    // Constructores
    
    public TipoDeporte() { }

    public TipoDeporte(int id, string nombre, string nombreTipoDeporte, string competiciones, List<Material> materiales, string descripcion)
{
    Id = id;
    Nombre = nombre;
    NombreTipoDeporte = nombreTipoDeporte;
    Competiciones = competiciones;
    Materiales = materiales;
    Descripcion = descripcion;
    Pistas = new List<Pista>(); 
}

    //metodo equals
    
   public override bool Equals(object? obj)
{
    if (obj == null || GetType() != obj.GetType())
        return false;

    var other = (TipoDeporte)obj;
    return Id == other.Id &&
           Nombre == other.Nombre &&
           NombreTipoDeporte == other.NombreTipoDeporte &&
           Competiciones == other.Competiciones &&
           Materiales == other.Materiales &&
           Descripcion == other.Descripcion;
}

    
}
