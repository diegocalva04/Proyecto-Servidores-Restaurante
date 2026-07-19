using System.Globalization;
using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Errors;

namespace Domain.ValueObjects;

/// Representa un correo electrónico válido e inmutable.
public sealed partial class CorreoElectronico : ValueObject
{
    private static readonly Regex EmailRegex = MyRegex();

    private CorreoElectronico(string valor)
    {
        Valor = valor;
    }

    public string Valor { get; }

    public static Result<CorreoElectronico> Crear(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            return Result.Failure<CorreoElectronico>(DomainErrors.Cliente.CorreoInvalido);
        }

        var normalizado = correo.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizado))
        {
            return Result.Failure<CorreoElectronico>(DomainErrors.Cliente.CorreoInvalido);
        }

        return Result.Success(new CorreoElectronico(normalizado));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Valor;
    }

    public override string ToString() => Valor;

    [GeneratedRegex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    )]
    private static partial Regex MyRegex();
}
