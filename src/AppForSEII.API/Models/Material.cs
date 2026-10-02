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

    // Relación N-1 con TipoDeporte
public TipoDeporte TipoDeporte { get; set; }

// Relación N-1 con TipoMaterial
public TipoMaterial TipoMaterial { get; set; }

// Relación 1-N con MaterialAlquilado
public List<MaterialAlquilado> MaterialesAlquilados { get; set; }

//constructores
    public Material()
{
}

public Material(int idMaterial, string nombre, int cantidad, double precio, TipoDeporte tipoDeporte,TipoMaterial tipoMaterial)
{
    IdMaterial = idMaterial;
    Nombre = nombre;
    Cantidad = cantidad;
    Precio = precio;
    TipoDeporte = tipoDeporte;
    TipoMaterial = tipoMaterial;
     MaterialesAlquilados = new List<MaterialAlquilado>();
}
//metodo equals
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