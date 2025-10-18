namespace LjudButikenBackEnd.DTOs;

using System.ComponentModel.DataAnnotations;
using LjudButikenBackEnd.Domain;

public record CategoryDto(
    int Id,
    string Name,
    string? Image
);


public record CategoryWithProductsDto(
    int Id,
    string Name,
    string? Image,
    List<ProductDto> Products
);


public record CategoryDetailsDto(
    int Id,
    string Name,
    string? Image,
    string Slug,
    List<ProductDto> Products
);

public record CategoryCreateDto(
    [param: Required, MinLength(1)] string Name,
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