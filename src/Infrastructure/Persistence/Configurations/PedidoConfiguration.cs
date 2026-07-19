using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// Configuración EF Core para el agregado <see cref="Pedido"/> y sus líneas.
public sealed class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ClienteId).HasColumnName("cliente_id").IsRequired();

        builder.Property(p => p.Fecha).HasColumnName("fecha").IsRequired();

        builder
            .Property(p => p.Estado)
            .HasColumnName("estado")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(p => p.Total)
            .HasColumnName("total")
            .HasConversion(ValueConverters.Precio)
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .HasMany(p => p.Lineas)
            .WithOne()
            .HasForeignKey("PedidoId")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Navigation(p => p.Lineas)
            .HasField("_lineas")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

internal sealed class PedidoLineaConfiguration : IEntityTypeConfiguration<PedidoLinea>
{
    public void Configure(EntityTypeBuilder<PedidoLinea> builder)
    {
        builder.ToTable("pedido_lineas");

        builder.HasKey(l => l.Id);

        builder.Property<Guid>("PedidoId").HasColumnName("pedido_id").IsRequired();

        builder.Property(l => l.PlatoId).HasColumnName("plato_id").IsRequired();

        builder
            .Property(l => l.NombrePlato)
            .HasColumnName("nombre_plato")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.Cantidad).HasColumnName("cantidad").IsRequired();

        builder
            .Property(l => l.PrecioUnitario)
            .HasColumnName("precio_unitario")
            .HasConversion(ValueConverters.Precio)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
