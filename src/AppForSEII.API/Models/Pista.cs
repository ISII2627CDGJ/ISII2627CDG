namespace AppForSEII.API.Models;


public class Pista
{   
     [Key]
    public int IdPista { get; set; }

    [Required]
    [StringLength(100)]
    public string NombrePista { get; set; } = string.Empty;

    [Required]
    public string NPersonas { get; set; } = string.Empty;

     [Required]
    [Range(0, 10000)]
    public double Precio { get; set; }  
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    //Relaciones

    //Tipo deporte principal
    public TipoDeporte TipoDeporte { get; set; }
    
    // Relación 1:N hacia PistaReservada
    public List<PistaReservada> PistasReservadas { get; set; }  

    //constructores
    public Pista()
    {
        
    }
    public Pista(int idPista, string nombrePista, string nPersonas, double precio, int stock)
    {
        IdPista = idPista;
        NombrePista = nombrePista;
        NPersonas = nPersonas;
        Precio = precio;
        Stock = stock;
    }   

    //equals
    public override bool Equals(object? obj)
    {
        if (obj is Pista other)
        {
            return IdPista == other.IdPista &&
                   NombrePista == other.NombrePista &&
                   NPersonas == other.NPersonas &&
                   Precio == other.Precio &&
                   Stock == other.Stock;
        }
        return false;
    }   
     
}