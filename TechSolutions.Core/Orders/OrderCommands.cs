using System;

namespace TechSolutions.Core.Orders
{
    public interface IOrderCommand
    {
        string Description { get; }

        void Execute(Order order);
        void Undo(Order order);
    }

    // ---- Create ----
    public class CreateOrderCommand : IOrderCommand
    {
        public string CustomerName { get; }
        public decimal Amount { get; }
        public string Details { get; }

        public string Description => $"Crear orden para {CustomerName}";

        public CreateOrderCommand(string customerName, decimal amount, string details)
        {
            CustomerName = customerName;
            Amount = amount;
            Details = details;
        }

        public void Execute(Order order)
        {
            // La orden ya fue creada por el servicio.
        }

        public void Undo(Order order)
        {
            // Deshacer creación: marcamos como cancelada.
            order.Cancel();
        }
    }

    // ---- Process ----
    public class ProcessOrderCommand : IOrderCommand
    {
        public string Description => "Procesar orden";

        public void Execute(Order order)
        {
            order.MarkProcessed();
        }

        public void Undo(Order order)
        {
            order.Restore(new OrderMemento(
                OrderStatus.Pending,
                order.Amount,
                order.DiscountPercentage));
        }
    }

    // ---- Cancel ----
    public class CancelOrderCommand : IOrderCommand
    {
        public string Description => "Cancelar orden";

        public void Execute(Order order)
        {
            order.Cancel();
        }

        public void Undo(Order order)
        {
            order.Restore(new OrderMemento(
                OrderStatus.Pending,
                order.Amount,
                order.DiscountPercentage));
        }
    }

    // ---- Apply Discount ----
    public class ApplyDiscountOrderCommand : IOrderCommand
    {
        private readonly decimal _percentage;
        private decimal _previousPercentage;

        public string Description => $"Aplicar descuento de {_percentage}%";

        public ApplyDiscountOrderCommand(decimal percentage)
        {
            _percentage = percentage;
        }

        public void Execute(Order order)
        {
            _previousPercentage = order.DiscountPercentage;
            order.ApplyDiscount(_percentage);
        }

        public void Undo(Order order)
        {
            order.ApplyDiscount(_previousPercentage);
        }
    }
}
