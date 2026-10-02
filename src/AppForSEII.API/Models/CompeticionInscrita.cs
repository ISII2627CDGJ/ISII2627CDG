using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(CompeticionId), nameof(InscripcionId))]
public class CompeticionInscrita
{
    public int CompeticionId { get; set; }
    public Competicion Competicion { get; set; } = null!;

    public int InscripcionId { get; set; }
    public InscripcionComp Inscripcion { get; set; } = null!;

    [StringLength(200)]
    public string? ProblemasFisicos { get; set; }

    public CompeticionInscrita() { }

    public CompeticionInscrita(int competicionId, int inscripcionId, string? problemasFisicos)
    {
        CompeticionId = competicionId;
        InscripcionId = inscripcionId;
        ProblemasFisicos = problemasFisicos;
    }

    public override bool Equals(object? obj)
    {
        return obj is CompeticionInscrita other &&
               CompeticionId == other.CompeticionId &&
               InscripcionId == other.InscripcionId &&
               ProblemasFisicos == other.ProblemasFisicos;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CompeticionId, InscripcionId, ProblemasFisicos);
    }
}