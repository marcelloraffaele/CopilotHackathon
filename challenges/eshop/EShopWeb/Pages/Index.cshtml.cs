using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShopWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShopWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ProductService _productService;
    public List<AutomobilePart> Products { get; set; } = new();
    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }
    [BindProperty(SupportsGet = true)]
    public decimal? MinPrice { get; set; }
    [BindProperty(SupportsGet = true)]
    public decimal? MaxPrice { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task OnGetAsync()
    {
        Products = await _productService.GetProductsAsync(Search, MinPrice, MaxPrice);
    }
}
