namespace TechSolutions.API.Dtos
{
    public class AdjustStockRequest
    {
        public int ProductId { get; set; }
        /// <summary>
        /// Cambio en el stock. Negativo para venta/consumo, positivo para ingreso.
        /// </summary>
        public int Delta { get; set; }
    }
}
