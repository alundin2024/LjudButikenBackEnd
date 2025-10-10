namespace LjudButikenBackEnd.Domain;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    
    public string? Image { get; set; }
    
    public string UrlSlug { get; set; } = string.Empty;

    public List<Category> Categories { get; set; } = new();
    
    //public DateTime? DeadLine { get; set; }
}
