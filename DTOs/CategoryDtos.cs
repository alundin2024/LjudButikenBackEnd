namespace LjudButikenBackEnd.DTOs;

public record CategoryDto(
    int Id,
    string Name
    );
//Used for /api/categories (list) and /api/categories?slug=
public record CategoryWithProductsDto(
    int Id,
    string Name,
    List<ProductDto> Products
);

// Used for /api/categories/{id} (includes slug per spec)
public record CatgoryDetailsDto(
    int Id,
    string Name,
    string Slug,
    List<ProductDto> Products
);

public record CategoryCreateDto(
    string Name
);


    