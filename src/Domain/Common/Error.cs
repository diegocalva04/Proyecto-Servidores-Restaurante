namespace Domain.Common;

/// Representa un error de dominio con código y mensaje descriptivo.
public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
