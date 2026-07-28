using Domain.ValueObjects;
using FluentAssertions;

namespace Tests.Domain;

public class ValueObjectTests
{
    [Fact]
    public void Precio_Valido_SeRedondea() => Precio.Crear(10.126m).Value.Valor.Should().Be(10.13m);

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Precio_NoPositivo_Falla(decimal valor) => Precio.Crear(valor).IsFailure.Should().BeTrue();

    [Fact]
    public void Correo_Valido_SeNormaliza() =>
        CorreoElectronico.Crear(" USER@EXAMPLE.COM ").Value.Valor.Should().Be("user@example.com");

    [Fact]
    public void Correo_Invalido_Falla() => CorreoElectronico.Crear("invalido").IsFailure.Should().BeTrue();

    [Fact]
    public void Telefono_Valido_Funciona() => Telefono.Crear("+593 999 123 456").IsSuccess.Should().BeTrue();

    [Fact]
    public void Telefono_Invalido_Falla() => Telefono.Crear("12").IsFailure.Should().BeTrue();

    [Fact]
    public void ValueObjects_ConMismoValor_SonIguales() =>
        Precio.Crear(12m).Value.Should().Be(Precio.Crear(12m).Value);

    [Fact]
    public void ValueObjects_SonInmutables() =>
        typeof(Precio).GetProperties().Should().OnlyContain(p => p.SetMethod == null);
}
