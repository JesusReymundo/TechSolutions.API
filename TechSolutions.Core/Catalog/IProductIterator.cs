using TechSolutions.Core.Entities;

namespace TechSolutions.Core.Catalog
{
    public interface IProductIterator
    {
        bool HasNext();
        Product Next();
    }
}
