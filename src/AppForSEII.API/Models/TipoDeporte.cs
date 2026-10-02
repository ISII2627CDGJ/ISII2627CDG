namespace AppForSEII.API.Models;


public class TipoDeporte
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Competiciones { get; set; } = string.Empty;

    [Required]
    public string Materiales { get; set; } = string.Empty;

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

    public TipoDeporte(int id, string nombre, string nombreTipoDeporte, string competiciones, string materiales, string descripcion)
    {
        Id = id;
        Nombre = nombre;
        NombreTipoDeporte = nombreTipoDeporte;
        Competiciones = competiciones;
        Materiales = materiales;
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
               Materiales == other.Materiales &&
               Descripcion == other.Descripcion;
    }
    

    
}