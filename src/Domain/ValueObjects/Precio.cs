using Domain.Common;
using Domain.Errors;

namespace Domain.ValueObjects;

/// Representa un precio monetario no negativo e inmutable.
public sealed class Precio : ValueObject
{
    private Precio(decimal valor)
    {
        Valor = valor;
    }

    public decimal Valor { get; }

    public static Result<Precio> Crear(decimal valor)
    {
        if (valor <= 0)
        {
            return Result.Failure<Precio>(DomainErrors.Plato.PrecioInvalido);
        }

        var redondeado = decimal.Round(valor, 2, MidpointRounding.AwayFromZero);
        return Result.Success(new Precio(redondeado));
    }

    public static Precio Zero => new(0m);

    public Precio Sumar(Precio otro) =>
        new(decimal.Round(Valor + otro.Valor, 2, MidpointRounding.AwayFromZero));

    public Precio Multiplicar(int cantidad)
    {
        if (cantidad < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(cantidad),
                "La cantidad no puede ser negativa."
            );
        }

        return new Precio(decimal.Round(Valor * cantidad, 2, MidpointRounding.AwayFromZero));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Valor;
    }

    public override string ToString() => Valor.ToString("F2");
}
