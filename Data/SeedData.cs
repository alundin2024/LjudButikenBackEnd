using LjudButikenBackEnd.Domain;
using Microsoft.EntityFrameworkCore;

namespace LjudButikenBackEnd.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(ApplicationDbContext db)
    {

        var soundboks = await db.Products.FirstOrDefaultAsync(p => p.UrlSlug == "soundboks4");
        if (soundboks is null)
        {
            soundboks = new Product
            {
                Name = "SOUNDBOKS 4",
                Description = "R�stark Bluetooth-h�gtalare med batteri f�r partymusik i 40 timmar",
                Price = 11990m,
                Image = "https://images.hifiklubben.com/image/a12197ce-e754-466c-878e-de77144c0222",
                UrlSlug = "soundboks4"
            };
            db.Products.Add(soundboks);
            await db.SaveChangesAsync();
        }

        var speakers = await db.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Slug == "hogtalare");
        if (speakers is null)
        {
            speakers = new Category
            {
                Name = "H�gtalare",
                Slug = "hogtalare",
                Image = "/images/speakers.png"
            };
            db.Categories.Add(speakers);
            await db.SaveChangesAsync();
        }

        if (!speakers.Products.Any(p => p.Id == soundboks.Id))
        {

            if (db.Entry(soundboks).State == EntityState.Detached)
            {
                db.Attach(soundboks);
            }
            speakers.Products.Add(soundboks);
            await db.SaveChangesAsync();
        }
    }
}
