namespace AppForSEII.API.Models;

public class PistaReservada
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Cantidad { get; set; }


    [Required]
    public int IdPista { get; set; }

    [Required]
    public int IdReserva { get; set; }

    public string Observaciones { get; set; }

    [Required]
    public decimal Precio { get; set; }

    /*
    relaciones 1..N siendo PistaReservada la 
    clase dependiente y las clases Pista 
    y Reserva las clases principales/padre
    */
    public Pista Pista { get; set; }
    public Reserva Reserva { get; set; }

    public PistaReservada()
    {
    }


    public PistaReservada(int id, int cantidad, int idPista, int idReserva, string observaciones, decimal precio)
    {
        Id = id;
        Cantidad = cantidad;
        IdPista = idPista;
        IdReserva = idReserva;
        Observaciones = observaciones;
        Precio = precio;
    }           
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (PistaReservada)obj;
            return Id == other.Id &&
                   Cantidad == other.Cantidad &&
                   IdPista == other.IdPista &&
                   IdReserva == other.IdReserva &&
                   Observaciones == other.Observaciones &&
                   Precio == other.Precio;
        }
    
}