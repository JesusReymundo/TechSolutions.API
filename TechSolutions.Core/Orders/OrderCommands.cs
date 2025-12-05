using System;

namespace TechSolutions.Core.Orders
{
    public class ProcessOrderCommand : IOrderCommand
    {
        public string Name => "Process";

        public void Execute(Order order, OrderHistory history)
        {
            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("No se puede procesar un pedido cancelado.");

            history.SaveState(order);
            order.Status = OrderStatus.Processed;
        }
    }

    public class CancelOrderCommand : IOrderCommand
    {
        public string Name => "Cancel";

        public void Execute(Order order, OrderHistory history)
        {
            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("El pedido ya está cancelado.");

            history.SaveState(order);
            order.Status = OrderStatus.Cancelled;
        }
    }

    public class ApplyDiscountCommand : IOrderCommand
    {
        private readonly decimal _discount;

        public ApplyDiscountCommand(decimal discount)
        {
            _discount = discount;
        }

        public string Name => "ApplyDiscount";

        public void Execute(Order order, OrderHistory history)
        {
            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("No se puede aplicar descuento a un pedido cancelado.");

            if (_discount < 0m || _discount > 0.90m)
                throw new ArgumentOutOfRangeException(nameof(_discount),
                    "El descuento debe estar entre 0% y 90%.");

            history.SaveState(order);
            order.DiscountPercentage = _discount;
        }
    }
}
