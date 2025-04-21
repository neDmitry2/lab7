using lab7.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int? categoryId,
            [FromQuery] string categoryName,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? inStock,
            [FromQuery] string sortBy = "Name",
            [FromQuery] string sortOrder = "asc")
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category);

            // Фильтрация по категории
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            else if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category.Name == categoryName);
            }

            // Фильтрация по цене
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Фильтрация по наличию
            if (inStock.HasValue)
            {
                query = query.Where(p => p.InStock == inStock.Value);
            }

            // Сортировка
            if (!string.IsNullOrEmpty(sortBy))
            {
                string direction = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";

                // Поддержка сортировки по полям связанной сущности
                if (sortBy.Contains("Category."))
                {
                    var catNameSort = sortBy.Split(".")[1];
                    query = query.OrderBy(p => p.Category.Name == catNameSort);
                }
                else
                {
                    query = sortBy switch
                    {
                        "Price" => query.OrderBy(p => p.Price),
                        "CreatedDate" => query.OrderBy(p => p.CreatedDate),
                        "Name" => query.OrderBy(p => p.Name),
                        _ => query
                    };
                }
            }

            var products = await query.ToListAsync();
            return Ok(products);
        }
    }
}
