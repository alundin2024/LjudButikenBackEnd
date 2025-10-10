using System.Globalization;
using System.Text;
using LjudButikenBackEnd.Data;
using LjudButikenBackEnd.Domain;
using LjudButikenBackEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LjudButikenBackEnd.Controllers;

// api/Products
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ProductsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/products and GET /api/products?slug=foo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] string? slug)
    {
        IQueryable<Product> query = _db.Products;

        if (!string.IsNullOrWhiteSpace(slug))
        {
            query = query.Where(p => p.UrlSlug == slug);
        }

        var products = await query
            .AsNoTracking()
            .ToListAsync();

        return Ok(products.Select(p => p.ToDto()));
    }

    // GET /api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return NotFound();

        return Ok(product.ToDto());
    }

    // POST /api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreateDto dto)
    {
        // [ApiController] handles basic validation (400) for required fields.
        var baseSlug = Slugify(dto.Name);
        var uniqueSlug = baseSlug;
        var i = 2;
        while (await _db.Products.AnyAsync(p => p.UrlSlug == uniqueSlug))
        {
            uniqueSlug = $"{baseSlug}-{i}";
            i++;
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Image = dto.Image,
            UrlSlug = uniqueSlug
        };

        // Attach categories if provided
        if (dto.Categories?.Count > 0)
        {
            var cats = await _db.Categories
                .Where(c => dto.Categories.Contains(c.Id))
                .ToListAsync();
            product.Categories = cats;
        }

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        var resultDto = product.ToDto();
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, resultDto);
    }

    // DELETE /api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null)
            return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static string Slugify(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                var ch = c;
                if (char.IsLetterOrDigit(ch)) sb.Append(ch);
                else if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_' || ch == '+') sb.Append('-');
            }
        }
        var slug = sb.ToString().Normalize(NormalizationForm.FormC);
        // collapse multiple dashes
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        return slug.Trim('-');
    }
}
