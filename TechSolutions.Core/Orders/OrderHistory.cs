using System;
using System.Collections.Generic;

namespace TechSolutions.Core.Orders
{
    /// <summary>
    /// Guarda el historial de comandos y snapshots (Command + Memento).
    /// </summary>
    public class OrderCommandHistory
    {
        private readonly Stack<(Guid orderId, OrderMemento snapshot, IOrderCommand command)> _history
            = new Stack<(Guid, OrderMemento, IOrderCommand)>();

        public void SaveSnapshot(Order order, IOrderCommand command)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (command == null) throw new ArgumentNullException(nameof(command));

            var memento = order.CreateMemento();
            _history.Push((order.Id, memento, command));
        }

        public bool TryRestoreLastSnapshot(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (_history.Count == 0) return false;

            var (orderId, snapshot, command) = _history.Peek();
            if (orderId != order.Id) return false;

            _history.Pop();

            order.Restore(snapshot);
            command.Undo(order);

            return true;
        }
    }
}
