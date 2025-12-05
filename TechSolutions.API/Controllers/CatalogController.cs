using Microsoft.AspNetCore.Mvc;
using TechSolutions.Core.Catalog;
using TechSolutions.Core.Entities;

namespace TechSolutions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // GET: api/Catalog?pageNumber=1&pageSize=3&nameFilter=Plan
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetPage(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 3,
            [FromQuery] string? nameFilter = null)
        {
            var page = _catalogService.GetPage(pageNumber, pageSize, nameFilter);

            return Ok(new
            {
                pageNumber,
                pageSize,
                nameFilter,
                items = page
            });
        }
    }
}
