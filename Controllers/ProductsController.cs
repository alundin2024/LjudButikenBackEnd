using Microsoft.AspNetCore.Mvc;

namespace LjudButikenBackEnd.Controllers;


// api/Products
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Products> GetProducts()

    {

    }
}
