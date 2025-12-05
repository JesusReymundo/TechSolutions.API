using System.Collections.Generic;

namespace TechSolutions.Core.Orders
{
    public class OrderHistory
    {
        private readonly Dictionary<int, Stack<OrderMemento>> _history = new();

        public void SaveState(Order order)
        {
            if (!_history.TryGetValue(order.Id, out var stack))
            {
                stack = new Stack<OrderMemento>();
                _history[order.Id] = stack;
            }

            stack.Push(new OrderMemento(order.Status, order.Amount, order.DiscountPercentage));
        }

        public bool TryRestoreLastState(Order order)
        {
            if (!_history.TryGetValue(order.Id, out var stack) || stack.Count == 0)
                return false;

            var memento = stack.Pop();
            order.Status = memento.Status;
            order.Amount = memento.Amount;
            order.DiscountPercentage = memento.DiscountPercentage;
            return true;
        }
    }
}
