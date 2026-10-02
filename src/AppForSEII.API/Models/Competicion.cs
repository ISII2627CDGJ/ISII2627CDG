
using System.ComponentModel.DataAnnotations;
namespace AppForSEII.API.Models;

public class Competicion
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Nombre { get; set; }
    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [StringLength(100)]
    public string Lugar { get; set; }

    [Range(0, int.MaxValue)]
    public int Plazas { get; set; }

    [Required]
    [Precision(5, 2)]
    public decimal Precio { get; set; }

    public int TipoDeporteId { get; set; }
    public TipoDeporte TipoDeporte { get; set; }
    public Competicion(){}
    public Competicion(string nombre, DateTime fecha, string lugar, int plazas, decimal precio, int tipoDeporteId)
    {
        Nombre = nombre;
        Fecha = fecha;
        Lugar = lugar;
        Plazas = plazas;
        Precio = precio;
        TipoDeporteId = tipoDeporteId;
    }
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }
}
