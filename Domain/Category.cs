namespace LjudButikenBackEnd.Domain;

public class Category
{
   public int Id { get; set; }

   public string Name { get; set; } = string.Empty;

   public string Slug { get; set; } = string.Empty;

   public string? Image { get; set; }

   public List<Product> Products { get; set; } = new();
}