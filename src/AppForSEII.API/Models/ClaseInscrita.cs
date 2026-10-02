public class ClaseInscrita
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PlazasReservadas { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public string? Observaciones { get; set; }

        
        public int ClaseDeportivaId { get; set; }
        public ClaseDeportiva ClaseDeportiva { get; set; }

      
        public int InscripcionId { get; set; }
       // public Inscripcion Inscripcion { get; set; }

        public ClaseInscrita()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is ClaseInscrita other)
            {
                return Id == other.Id &&
                       PlazasReservadas == other.PlazasReservadas &&
                       Precio == other.Precio &&
                       Observaciones == other.Observaciones &&
                       ClaseDeportivaId == other.ClaseDeportivaId &&
                       InscripcionId == other.InscripcionId;
            }
            return false;
        }
    }