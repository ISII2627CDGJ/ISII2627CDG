namespace AppForSEII.API.Models;


public class TipoDeporte
{
    [Key]
    public int Id { get; set; }

    [Required]
    public List <Competicion> Competiciones { get; set; } = new List<Competicion>();



    [Required]
     [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
    public string Nombre { get; set; }

    [Required]
     [StringLength(50, ErrorMessage = "El nombre del tipo de deporte no puede tener más de 50 caracteres.")]
    public string NombreTipoDeporte { get; set; }

    [Required]
    public string Descripcion { get; set; }

    // Relación 1-N con Material
    public List<Material> Materiales { get; set; }

    //Relacion 1--N con pista
    public List<Pista> Pistas { get; set; }

    //Relacion 1:N con ClaseDeportiva
    public List<ClaseDeportiva> ClasesDeportivas { get; set; } = new List<ClaseDeportiva>();

    // Constructores
    
    public TipoDeporte() { }

    public TipoDeporte(int id, string nombre, string nombreTipoDeporte, string competiciones, string descripcion)
{
    Id = id;
    Nombre = nombre;
    NombreTipoDeporte = nombreTipoDeporte;
    Descripcion = descripcion;
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
           Descripcion == other.Descripcion;
}

    
}
