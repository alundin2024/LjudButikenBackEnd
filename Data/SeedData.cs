using LjudButikenBackEnd.Domain;
using Microsoft.EntityFrameworkCore;

namespace LjudButikenBackEnd.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(ApplicationDbContext db)
    {
        // Apply migrations if pending
        if ((await db.Database.GetPendingMigrationsAsync()).Any())
        {
            await db.Database.MigrateAsync();
        }

        if (!await db.Products.AnyAsync(p => p.UrlSlug == "soundboks4"))
        {
            db.Products.Add(new Product
            {
                Name = "SOUNDBOKS 4",
                Description = "Råstark Bluetooth-högtalare med batteri för partymusik i 40 timmar",
                Price = 11990m,
                Image = "https://images.hifiklubben.com/image/a12197ce-e754-466c-878e-de77144c0222",
                UrlSlug = "soundboks4"
            });
            await db.SaveChangesAsync();
        }
    }
}
