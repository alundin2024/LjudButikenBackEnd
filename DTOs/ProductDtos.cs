namespace LjudButikenBackEnd.DTOs;

public record ProductDto
(
    int id,
    string Name,
    string? Description,
    decimal Price,
    string UrlSlug
    );
    
    public record ProductCreateDto
    (
        string Name,
        string? Description,
        decimal Price,
        List<int> Categories
        );