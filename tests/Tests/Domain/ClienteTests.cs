using Domain.Entities;
using Domain.Events;
using FluentAssertions;

namespace Tests.Domain;

public class ClienteTests
{
    [Fact]
    public void Crear_ConDatosValidos_GeneraIdentidadYEvento()
    {
        var result = Cliente.Crear("Ana", "Pérez", "ANA@correo.com", "+593 999 123 456");

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Correo.Valor.Should().Be("ana@correo.com");
        result.Value.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<ClienteRegistrado>();
    }

    [Theory]
    [InlineData("", "Pérez", "ana@correo.com", "0999123456", "Cliente.NombreInvalido")]
    [InlineData("Ana", "Pérez", "correo", "0999123456", "Cliente.CorreoInvalido")]
    [InlineData("Ana", "Pérez", "ana@correo.com", "123", "Cliente.TelefonoInvalido")]
    public void Crear_ConDatosInvalidos_Falla(
        string nombre, string apellido, string correo, string telefono, string codigo)
    {
        Cliente.Crear(nombre, apellido, correo, telefono).Error.Code.Should().Be(codigo);
    }

    [Fact]
    public void Actualizar_ConDatosValidos_CambiaLosDatos()
    {
        var cliente = CrearCliente();
        cliente.Actualizar("Luis", "Vega", "luis@correo.com", "0999555444").IsSuccess.Should().BeTrue();
        cliente.NombreCompleto.ValorCompleto.Should().Be("Luis Vega");
    }

    [Fact]
    public void Propiedades_NoTienenSetterPublico()
    {
        typeof(Cliente).GetProperties()
            .Where(p => p.SetMethod is not null)
            .Should().OnlyContain(p => !p.SetMethod!.IsPublic);
    }

    [Fact]
    public void LimpiarEventos_EliminaEventos()
    {
        var cliente = CrearCliente();
        cliente.ClearDomainEvents();
        cliente.DomainEvents.Should().BeEmpty();
    }

    private static Cliente CrearCliente() =>
        Cliente.Crear("Ana", "Pérez", "ana@correo.com", "0999123456").Value;
}
