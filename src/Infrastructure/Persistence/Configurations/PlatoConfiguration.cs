using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// Configuración EF Core para la entidad <see cref="Plato"/>.
public sealed class PlatoConfiguration : IEntityTypeConfiguration<Plato>
{
    public void Configure(EntityTypeBuilder<Plato> builder)
    {
        builder.ToTable("platos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre).HasColumnName("nombre").HasMaxLength(200).IsRequired();

        builder
            .Property(p => p.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(1000)
            .IsRequired();

        builder
            .Property(p => p.Categoria)
            .HasColumnName("categoria")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Disponible).HasColumnName("disponible").IsRequired();

        builder
            .Property(p => p.Precio)
            .HasColumnName("precio")
            .HasConversion(ValueConverters.Precio)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
