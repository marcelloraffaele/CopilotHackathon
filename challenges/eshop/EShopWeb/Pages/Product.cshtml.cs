using Microsoft.AspNetCore.Mvc.RazorPages;
using EShopWeb.Models;
using System.Threading.Tasks;

namespace EShopWeb.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ProductService _productService;
        public AutomobilePart? Product { get; set; }
        public ProductModel(ProductService productService)
        {
            _productService = productService;
        }
        public async Task OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id);
        }
    }
}
