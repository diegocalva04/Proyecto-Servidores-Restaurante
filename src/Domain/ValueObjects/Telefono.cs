using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Errors;

namespace Domain.ValueObjects;

/// Representa un número telefónico válido e inmutable.
public sealed partial class Telefono : ValueObject
{
    private static readonly Regex TelefonoRegex = MyRegex();

    private Telefono(string valor)
    {
        Valor = valor;
    }

    public string Valor { get; }

    public static Result<Telefono> Crear(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
        {
            return Result.Failure<Telefono>(DomainErrors.Cliente.TelefonoInvalido);
        }

        var normalizado = telefono.Trim();

        if (!TelefonoRegex.IsMatch(normalizado))
        {
            return Result.Failure<Telefono>(DomainErrors.Cliente.TelefonoInvalido);
        }

        return Result.Success(new Telefono(normalizado));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Valor;
    }

    public override string ToString() => Valor;

    [GeneratedRegex(
        @"^\+?[0-9\s\-()]{7,20}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    )]
    private static partial Regex MyRegex();
}
