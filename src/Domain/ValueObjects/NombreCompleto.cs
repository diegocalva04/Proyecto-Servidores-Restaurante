using Domain.Common;
using Domain.Errors;

namespace Domain.ValueObjects;

/// Encapsula el nombre y apellido de una persona como un único concepto de dominio.
public sealed class NombreCompleto : ValueObject
{
    private NombreCompleto(string nombre, string apellido)
    {
        Nombre = nombre;
        Apellido = apellido;
    }

    public string Nombre { get; }

    public string Apellido { get; }

    public string ValorCompleto => $"{Nombre} {Apellido}";

    public static Result<NombreCompleto> Crear(string nombre, string apellido)
    {
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
        {
            return Result.Failure<NombreCompleto>(DomainErrors.Cliente.NombreInvalido);
        }

        return Result.Success(new NombreCompleto(nombre.Trim(), apellido.Trim()));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Nombre;
        yield return Apellido;
    }

    public override string ToString() => ValorCompleto;
}
