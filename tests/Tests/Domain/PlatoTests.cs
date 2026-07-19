using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Domain;

public class PlatoTests
{
    [Fact]
    public void Crear_DebeRetornarExito_CuandoLosDatosSonValidos()
    {
        var resultado = Plato.Crear("Paella", "Arroz con mariscos", 129.50m, CategoriaPlato.Principal);

        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Nombre.Should().Be("Paella");
        resultado.Value.Descripcion.Should().Be("Arroz con mariscos");
        resultado.Value.Precio.Valor.Should().Be(129.50m);
        resultado.Value.Categoria.Should().Be(CategoriaPlato.Principal);
        resultado.Value.Disponible.Should().BeTrue();
    }

    [Fact]
    public void Crear_DebeRetornarError_CuandoElNombreEsVacio()
    {
        var resultado = Plato.Crear(string.Empty, "Descripcion", 50m, CategoriaPlato.Entrada);

        resultado.IsFailure.Should().BeTrue();
        resultado.Error.Code.Should().Be("Plato.NombreInvalido");
    }

    [Fact]
    public void Actualizar_DebeModificarPropiedades_CuandoLosDatosSonValidos()
    {
        var plato = Plato.Crear("Ensalada", "Verde", 35m, CategoriaPlato.Entrada).Value;

        var resultado = plato.Actualizar("Ensalada César", "Lechuga y pollo", 39.90m, CategoriaPlato.Entrada, false);

        resultado.IsSuccess.Should().BeTrue();
        plato.Nombre.Should().Be("Ensalada César");
        plato.Descripcion.Should().Be("Lechuga y pollo");
        plato.Precio.Valor.Should().Be(39.90m);
        plato.Disponible.Should().BeFalse();
    }

    [Fact]
    public void MarcarComoNoDisponible_DebeMarcarElPlatoComoNoDisponible()
    {
        var plato = Plato.Crear("Sopa", "Caliente", 25m, CategoriaPlato.Entrada).Value;

        plato.MarcarComoNoDisponible();

        plato.Disponible.Should().BeFalse();
    }
}
