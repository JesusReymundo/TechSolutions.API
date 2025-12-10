using System.Collections.Generic;

namespace TechSolutions.Core.Payments
{
    /// <summary>
    /// Configuración de pagos (activa/inactiva, métodos permitidos, límites, etc.).
    /// </summary>
    public class PaymentConfiguration
    {
        /// <summary>
        /// Indica si el procesamiento de pagos está habilitado en el sistema.
        /// (esto es lo que usa el controlador: config.Enabled)
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Métodos de pago permitidos.
        /// </summary>
        public List<PaymentMethod> EnabledMethods { get; set; } =
            new List<PaymentMethod>
            {
                PaymentMethod.PayPal,
                PaymentMethod.Yape,
                PaymentMethod.Plin
            };

        /// <summary>
        /// Monto máximo permitido por transacción.
        /// </summary>
        public decimal MaxAmount { get; set; } = 10000m;

        /// <summary>
        /// Moneda por defecto.
        /// </summary>
        public string DefaultCurrency { get; set; } = "PEN";
    }
}

