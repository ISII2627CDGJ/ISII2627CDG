namespace AppForSEII.API.Models;

public class Inscripcion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime FechaInscripcion { get; set; }
        [Required]
        public decimal PrecioTotal { get; set; }
        [Required]
        public string DatosPago { get; set; }
        [Required]
        public MetodoPago MetodoPago { get; set; }

   


        //Relaciones 
        
        public IList<ClaseInscrita> ClasesInscritas { get; set; } = new List<ClaseInscrita>();
        public ApplicationUser Cliente { get; set; } = null!;
        

        public Inscripcion(int id, DateTime fechaInscripcion, decimal precioTotal, string datosPago, MetodoPago metodoPago, ApplicationUser cliente, IList<ClaseInscrita> clasesInscritas)
        {
            Id = id;
            FechaInscripcion = fechaInscripcion;
            PrecioTotal = precioTotal;
            DatosPago = datosPago;
            MetodoPago = metodoPago;
            Cliente = cliente;
            ClasesInscritas = clasesInscritas ?? new List<ClaseInscrita>();
        }


        public override bool Equals(object obj)
        {
            if (obj is Inscripcion other)
            {
            return Id == other.Id &&
                   FechaInscripcion == other.FechaInscripcion &&
                   PrecioTotal == other.PrecioTotal &&
                   DatosPago == other.DatosPago &&
                   MetodoPago == other.MetodoPago &&
                   EqualityComparer<ApplicationUser>.Default.Equals(Cliente, other.Cliente) &&
                   ClasesInscritas.SequenceEqual(other.ClasesInscritas);
            }
            return false;
        }

       
    }