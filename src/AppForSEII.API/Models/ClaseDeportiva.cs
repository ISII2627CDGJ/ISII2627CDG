namespace AppForSEII.API.Models;

public class ClaseDeportiva
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public DateTime FechaHora { get; set; }
        [Required]
        public string? Lugar { get; set; }
        [Required]
        public string Monitor { get; set; }
        [Required]
        public string Nivel { get; set; }
        [Required]
        public int PlazasDisponibles { get; set; }
        [Required]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public int TipoDeporteId { get; set; }
        public TipoDeporte TipoDeporte { get; set; }

       //Relaciones 
        public List<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();
        
      

        public ClaseDeportiva()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is ClaseDeportiva other)
            {
            return Id == other.Id &&
                   Descripcion == other.Descripcion &&
                   FechaHora == other.FechaHora &&
                   Lugar == other.Lugar &&
                   Monitor == other.Monitor &&
                   Nivel == other.Nivel &&
                   PlazasDisponibles == other.PlazasDisponibles &&
                   PrecioUnitario == other.PrecioUnitario &&
                   TipoDeporteId == other.TipoDeporteId;
            }
            return false;
        }
    }