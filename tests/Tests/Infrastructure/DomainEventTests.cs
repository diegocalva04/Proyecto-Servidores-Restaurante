using Domain.Common;
using Domain.Entities;
using Domain.Events;
using FluentAssertions;
using Infrastructure.DomainEvents;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests.Infrastructure;

public class DomainEventTests
{
    [Fact]
    public async Task Dispatcher_EjecutaHandlerRegistrado()
    {
        var handler = Substitute.For<IDomainEventHandler<ClienteRegistrado>>();
        var services = new ServiceCollection()
            .AddSingleton(handler)
            .BuildServiceProvider();
        var dispatcher = new DomainEventDispatcher(services);
        var domainEvent = new ClienteRegistrado(
            Guid.NewGuid(), "Ana", "Pérez", "ana@correo.com", DateTimeOffset.UtcNow);

        await dispatcher.DispatchAsync([domainEvent]);

        await handler.Received(1).HandleAsync(domainEvent, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UnitOfWork_DespachaYLimpiarEventos_TrasPersistir()
    {
        var dispatcher = Substitute.For<IDomainEventDispatcher>();
        await using var context = CrearContexto();
        var cliente = Cliente.Crear("Ana", "Pérez", "ana@correo.com", "0999123456").Value;
        context.Clientes.Add(cliente);

        await new UnitOfWork(context, dispatcher).GuardarCambiosAsync();

        await dispatcher.Received(1).DispatchAsync(
            Arg.Is<IEnumerable<IDomainEvent>>(events => events.Single() is ClienteRegistrado),
            Arg.Any<CancellationToken>());
        cliente.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task UnitOfWork_NoDespachaNiLimpia_CuandoSaveChangesFalla()
    {
        var dispatcher = Substitute.For<IDomainEventDispatcher>();
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<RestauranteDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        var cliente = Cliente.Crear("Ana", "Pérez", "ana@correo.com", "0999123456").Value;
        await using (var firstContext = new RestauranteDbContext(options))
        {
            firstContext.Clientes.Add(cliente);
            await firstContext.SaveChangesAsync();
        }

        await using var context = new RestauranteDbContext(options);
        context.Clientes.Add(cliente);

        var action = () => new UnitOfWork(context, dispatcher).GuardarCambiosAsync();

        await action.Should().ThrowAsync<ArgumentException>();
        await dispatcher.DidNotReceive().DispatchAsync(
            Arg.Any<IEnumerable<IDomainEvent>>(), Arg.Any<CancellationToken>());
        cliente.DomainEvents.Should().ContainSingle();
    }

    private static RestauranteDbContext CrearContexto()
    {
        var options = new DbContextOptionsBuilder<RestauranteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RestauranteDbContext(options);
    }
}
