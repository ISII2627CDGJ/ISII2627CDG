using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(IdMaterial), nameof(IdAlquiler))]
public class MaterialAlquilado
{
    public int IdMaterial { get; set; }

    public int IdAlquiler { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
    public int Cantidad { get; set; }

    [StringLength(200)]
    public string? Descripcion { get; set; }

    [Required]
    [Precision(7, 2)]
    public decimal Precio { get; set; }
    public MaterialAlquilado()
{
}
public MaterialAlquilado(int idMaterial, int idAlquiler, int cantidad, string? descripcion, decimal precio)
{
    IdMaterial = idMaterial;
    IdAlquiler = idAlquiler;
    Cantidad = cantidad;
    Descripcion = descripcion;
    Precio = precio;
}
public override bool Equals(object? obj)
{
    if (obj is MaterialAlquilado other)
    {
        return IdMaterial == other.IdMaterial &&
               IdAlquiler == other.IdAlquiler &&
               Cantidad == other.Cantidad &&
               Descripcion == other.Descripcion &&
               Precio == other.Precio;
    }

    return false;
}
}