using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class TipoMaterial
{
    [Key]
    public int IdTipoMaterial { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreTipoMaterial { get; set; } = string.Empty;
    
    // Relación 1-N con Material
    public List<Material> Materiales { get; set; }
    
    //constructores
    public TipoMaterial()
    {
    }

    public TipoMaterial(int idTipoMaterial, string nombreTipoMaterial)
    {
        IdTipoMaterial = idTipoMaterial;
        NombreTipoMaterial = nombreTipoMaterial;
        Materiales = new List<Material>();
    }
    //metodo equals 
    public override bool Equals(object? obj)
    {
        if (obj is TipoMaterial other)
        {
            return IdTipoMaterial == other.IdTipoMaterial &&
                   NombreTipoMaterial == other.NombreTipoMaterial;
        }

        return false;
    }
}