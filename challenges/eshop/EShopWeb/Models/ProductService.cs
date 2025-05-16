using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EShopWeb.Models
{
    public class AutomobilePart
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image_Url { get; set; } = string.Empty;
    }

    public class ProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<AutomobilePart>> GetProductsAsync(string? search = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var url = "http://localhost:5002/api/eshop/parts";
            if (!string.IsNullOrEmpty(search) || minPrice.HasValue || maxPrice.HasValue)
            {
                url = $"http://localhost:5002/api/eshop/search?name={search}";
            }
            var products = await _httpClient.GetFromJsonAsync<List<AutomobilePart>>(url) ?? new List<AutomobilePart>();
            if (minPrice.HasValue)
                products = products.FindAll(p => p.Price >= minPrice);
            if (maxPrice.HasValue)
                products = products.FindAll(p => p.Price <= maxPrice);
            return products;
        }
        public async Task<AutomobilePart?> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<AutomobilePart>($"http://localhost:5002/api/eshop/parts/{id}");
        }
    }
}
