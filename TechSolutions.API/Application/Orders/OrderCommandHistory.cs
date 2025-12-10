using TechSolutions.API.Application.Orders.Commands;

namespace TechSolutions.API.Application.Orders;

public class OrderCommandHistory
{
    private readonly Stack<IOrderCommand> _commands = new();

    public void Push(IOrderCommand command) => _commands.Push(command);

    public IOrderCommand? Pop()
        => _commands.Count > 0 ? _commands.Pop() : null;

    public bool HasHistory => _commands.Count > 0;
}
