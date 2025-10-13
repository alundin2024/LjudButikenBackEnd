namespace LjudButikenBackEnd.DTOs;

using System.ComponentModel.DataAnnotations;
using LjudButikenBackEnd.Domain;

public record CategoryDto(
    int Id,
    string Name,
    string? Image
);

// Used for /api/categories (list) and /api/categories?slug=
public record CategoryWithProductsDto(
    int Id,
    string Name,
    string? Image,
    List<ProductDto> Products
);

// Used for /api/categories/{id} (includes slug per spec)
public record CategoryDetailsDto(
    int Id,
    string Name,
    string? Image,
    string Slug,
    List<ProductDto> Products
);

public record CategoryCreateDto(
    [parameter: Required, MinLength(1)] string Name,
    string? Image
);

public static class CategoryMappings
{
    public static CategoryDto ToDto(this Category c) => new(
        c.Id,
        c.Name,
        c.Image
    );

    public static CategoryDetailsDto ToDetailsDto(this Category c) => new(
        c.Id,
        c.Name,
        c.Image,
        c.Slug,
        c.Products?.Select(p => p.ToDto()).ToList() ?? new List<ProductDto>()
    );

    public static CategoryWithProductsDto ToWithProductsDto(this Category c) => new(
        c.Id,
        c.Name,
        c.Image,
        c.Products?.Select(p => p.ToDto()).ToList() ?? new List<ProductDto>()
    );
}