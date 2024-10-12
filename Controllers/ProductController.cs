using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    // Simulando um banco de dados com uma lista estática
    private static readonly List<Product> Products = new List<Product>
    {
        new Product { Id = 1, Name = "Product 1", Price = 10.0 },
        new Product { Id = 2, Name = "Product 2", Price = 20.0 },
    };

    // GET: api/product
    [HttpGet]
    public IEnumerable<Product> GetProducts()
    {
        return Products;
    }

    // GET: api/product/1
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = Products.Find(p => p.Id == id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/product
    [HttpPost]
    public IActionResult AddProduct(Product product)
    {
        product.Id = Products.Count + 1;
        Products.Add(product);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
}

