namespace LjudButikenBackEnd.DTOs;

using System.ComponentModel.DataAnnotations;
using LjudButikenBackEnd.Domain;

public record ProductDto
(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    string? Image,
    string UrlSlug
);

public record ProductCreateDto
(
    [param: Required, MinLength(1)] string Name,
    string? Description,
    [param: Range(typeof(decimal), "0", "79228162514264337593543950335")] decimal Price,
    string? Image,
    List<int>? Categories
);

public static class ProductMappings
{
    public static ProductDto ToDto(this Product p) => new(
        p.Id,
        p.Name,
        p.Description,
        p.Price,
        p.Image,
        p.UrlSlug
    );
}