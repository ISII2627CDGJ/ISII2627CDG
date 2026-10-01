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
    

    //Relacion 1--N con pista
    public List<Pista> Pistas { get; set; }

        // Constructores
        public TipoDeporte() { }

        public TipoDeporte(int id, string nombre, string nombreTipoDeporte, string competiciones, string materiales)
        {
            Id = id;
            Nombre = nombre;
            NombreTipoDeporte = nombreTipoDeporte;
            Competiciones = competiciones;
            Materiales = materiales;
        }
        public override bool Equals(object obj)
        {
            return obj is TipoDeporte deporte && Id == deporte.Id;
        }

       
    
     
}