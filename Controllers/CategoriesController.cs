
using System.Globalization;
using System.Text;
using LjudButikenBackEnd.Data;
using LjudButikenBackEnd.Domain;
using LjudButikenBackEnd.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LjudButikenBackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public CategoriesController(ApplicationDbContext db)
    {
        _db = db;
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryWithProductsDto>>> GetCategories([FromQuery] string? slug)
    {
        IQueryable<Category> query = _db.Categories
            .Include(c => c.Products);

        if (!string.IsNullOrWhiteSpace(slug))
        {
            query = query.Where(c => c.Slug == slug);
        }

        var categories = await query
            .AsNoTracking()
            .ToListAsync();

        return Ok(categories.Select(c => c.ToWithProductsDto()));
    }

    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDetailsDto>> GetCategoryById(int id)
    {
        var category = await _db.Categories
            .Include(c => c.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return NotFound();

        return Ok(category.ToDetailsDto());
    }

    // POST /api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryCreateDto dto)
    {
        var baseSlug = Slugify(dto.Name);
        var uniqueSlug = baseSlug;
        var i = 2;
        while (await _db.Categories.AnyAsync(c => c.Slug == uniqueSlug))
        {
            uniqueSlug = $"{baseSlug}-{i}";
            i++;
        }

        var category = new Category
        {
            Name = dto.Name,
            Image = dto.Image,
            Slug = uniqueSlug
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category.ToDto());
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null)
            return NotFound();

        _db.Categories.Remove(category);
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
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        return slug.Trim('-');
    }
}
