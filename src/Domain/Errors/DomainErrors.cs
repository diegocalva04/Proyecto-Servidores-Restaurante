namespace Domain.Errors;

/// Errores predefinidos del dominio del restaurante.
public static class DomainErrors
{
    public static class Cliente
    {
        public static readonly Common.Error NombreInvalido = new(
            "Cliente.NombreInvalido",
            "El nombre y apellido son obligatorios."
        );

        public static readonly Common.Error CorreoInvalido = new(
            "Cliente.CorreoInvalido",
            "El correo electrónico no es válido."
        );

        public static readonly Common.Error TelefonoInvalido = new(
            "Cliente.TelefonoInvalido",
            "El teléfono no es válido."
        );
    }

    public static class Plato
    {
        public static readonly Common.Error NombreInvalido = new(
            "Plato.NombreInvalido",
            "El nombre del plato es obligatorio."
        );

        public static readonly Common.Error PrecioInvalido = new(
            "Plato.PrecioInvalido",
            "El precio debe ser mayor o igual a cero."
        );

        public static readonly Common.Error NoDisponible = new(
            "Plato.NoDisponible",
            "El plato no está disponible."
        );
    }

    public static class Pedido
    {
        public static readonly Common.Error ClienteRequerido = new(
            "Pedido.ClienteRequerido",
            "El pedido debe estar asociado a un cliente válido."
        );

        public static readonly Common.Error SinPlatos = new(
            "Pedido.SinPlatos",
            "No se puede registrar un pedido sin platos."
        );

        public static readonly Common.Error PlatoNoDisponible = new(
            "Pedido.PlatoNoDisponible",
            "Uno o más platos solicitados no están disponibles."
        );

        public static readonly Common.Error PlatoNoEncontrado = new(
            "Pedido.PlatoNoEncontrado",
            "Uno o más platos solicitados no existen."
        );

        public static readonly Common.Error CantidadInvalida = new(
            "Pedido.CantidadInvalida",
            "La cantidad de cada plato debe ser mayor a cero."
        );

        public static readonly Common.Error YaEntregado = new(
            "Pedido.YaEntregado",
            "No se puede modificar un pedido que ya fue entregado."
        );

        public static readonly Common.Error EstadoInvalido = new(
            "Pedido.EstadoInvalido",
            "El estado del pedido no es válido."
        );
    }
}
