using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Material
{
    [Key]
    public int IdMaterial { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public int Cantidad { get; set; }

    [Required]
    public double Precio { get; set; }

    public Material()
{
}

public Material(int idMaterial, string nombre, int cantidad, double precio)
{
    IdMaterial = idMaterial;
    Nombre = nombre;
    Cantidad = cantidad;
    Precio = precio;
}

public override bool Equals(object? obj)
{
    if (obj is Material other)
    {
        return IdMaterial == other.IdMaterial &&
               Nombre == other.Nombre &&
               Cantidad == other.Cantidad &&
               Precio == other.Precio;
    }

    return false;
}
}