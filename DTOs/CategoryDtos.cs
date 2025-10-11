namespace LjudButikenBackEnd.DTOs;

using LjudButikenBackEnd.Domain;

public record CategoryDto(
    int Id,
    string Name
);

// Used for /api/categories (list) and /api/categories?slug=
public record CategoryWithProductsDto(
    int Id,
    string Name,
    List<ProductDto> Products
);

// Used for /api/categories/{id} (includes slug per spec)
public record CategoryDetailsDto(
    int Id,
    string Name,
    string Slug,
    List<ProductDto> Products
);

public record CategoryCreateDto(
    string Name
);

public static class CategoryMappings
{
    public static CategoryDto ToDto(this Category c) => new(
        c.Id,
        c.Name
    );

    public static CategoryDetailsDto ToDetailsDto(this Category c) => new(
        c.Id,
        c.Name,
        c.Slug,
        c.Products?.Select(p => p.ToDto()).ToList() ?? new List<ProductDto>()
    );

    public static CategoryWithProductsDto ToWithProductsDto(this Category c) => new(
        c.Id,
        c.Name,
        c.Products?.Select(p => p.ToDto()).ToList() ?? new List<ProductDto>()
    );
}