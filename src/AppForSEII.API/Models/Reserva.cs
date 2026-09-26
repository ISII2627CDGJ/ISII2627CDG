namespace AppForSEII.API.Models;


//completar los atributos
public class Reserva
{
    public int Id { get; set; }
    public DateTime FechaReserva { get; set; } = DateTime.Now;
    
   
    public string ClienteNombre { get; set; } = string.Empty;
    public string ClienteApellidos { get; set; } = string.Empty;
    public string ClienteDni { get; set; } = string.Empty;
    
    
    public double PrecioTotal { get; set; }
    
   
}