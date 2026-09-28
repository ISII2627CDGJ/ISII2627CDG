using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Material 
{
    [Key]
    public int IdMateria { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public double Precio { get; set; }
}