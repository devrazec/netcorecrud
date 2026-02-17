using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll(
     [FromQuery] int _page = 1,
     [FromQuery] int _limit = 10)
    {
        var query = _context.Products.AsQueryable();

        var total = await query.CountAsync();

        var data = await query
            .Skip((_page - 1) * _limit)
            .Take(_limit)
            .ToListAsync();

        // Set Content-Range header
        var start = (_page - 1) * _limit;
        var end = start + data.Count - 1;
        Response.Headers.Append("Content-Range", $"products {start}-{end}/{total}");

        return Ok(data);
    }


    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return product;
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Notify clients of change
        ProductHub.NotifyProductsChanged();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        _context.Entry(product).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        // Notify clients of change
        ProductHub.NotifyProductsChanged();

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        // Notify clients of change
        ProductHub.NotifyProductsChanged();

        return NoContent();
    }
}
