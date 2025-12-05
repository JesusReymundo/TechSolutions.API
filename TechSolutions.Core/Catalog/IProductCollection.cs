namespace TechSolutions.Core.Catalog
{
    public interface IProductCollection
    {
        IProductIterator CreateIterator(
            int pageNumber,
            int pageSize,
            string? nameFilter = null);
    }
}
