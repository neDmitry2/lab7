using lab7.Model;
using Microsoft.AspNetCore.Mvc;

namespace lab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SeedData()
        {
            if (_context.Categories.Any() || _context.Products.Any())
            {
                return Ok("Database already has data");
            }

            var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Electronic devices" },
                new Category { Name = "Furniture", Description = "Home and office furniture" },
                new Category { Name = "Stationery", Description = "Office supplies" }
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            var products = new List<Product>
            {
                new Product { Name = "Laptop", CategoryId = 1, Price = 999.99m, InStock = true },
                new Product { Name = "Smartphone", CategoryId = 1, Price = 699.99m, InStock = true },
                new Product { Name = "Headphones", CategoryId = 1, Price = 149.99m, InStock = false },
                new Product { Name = "Desk Chair", CategoryId = 2, Price = 199.99m, InStock = true },
                new Product { Name = "Coffee Table", CategoryId = 2, Price = 149.99m, InStock = true },
                new Product { Name = "Notebook", CategoryId = 3, Price = 4.99m, InStock = true },
                new Product { Name = "Pen", CategoryId = 3, Price = 1.99m, InStock = false }
            };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            return Ok("Test data seeded successfully");
        }
    }
}
