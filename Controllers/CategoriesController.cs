// csharp
using System.Linq;
using LjudButikenBackEnd.Data;
using LjudButikenBackEnd.Domain;
using LjudButikenBackEnd.DTOs;
using LjudButikenBackEnd.Services;
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
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] string? slug)
    {
        IQueryable<Category> query = _db.Categories;

        if (!string.IsNullOrWhiteSpace(slug))
        {
            query = query.Where(c => c.Slug == slug);
        }

        var categories = await query
            .AsNoTracking()
            .ToListAsync();

        return Ok(categories.Select(c => c.ToDto()));
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

    
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryCreateDto dto)
    {
        var baseSlug = Slugify.Generate(dto.Name);
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
            Slug = uniqueSlug
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        var resultDto = category.ToDto();
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, resultDto);
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
}
