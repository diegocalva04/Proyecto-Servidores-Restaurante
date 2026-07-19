using Domain.Entities;
using Infrastructure.Persistence.Conversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// Configuración EF Core para la entidad <see cref="Cliente"/>.
public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.NombreCompleto)
            .HasColumnName("nombre_completo")
            .HasConversion(ValueConverters.NombreCompleto)
            .HasMaxLength(400)
            .IsRequired();

        builder
            .Property(c => c.Correo)
            .HasColumnName("correo")
            .HasConversion(ValueConverters.Correo)
            .HasMaxLength(320)
            .IsRequired();

        builder
            .Property(c => c.Telefono)
            .HasColumnName("telefono")
            .HasConversion(ValueConverters.Telefono)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(c => c.Correo).IsUnique();
    }
}
