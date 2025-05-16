using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EShopAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EShopController : ControllerBase
{
    private readonly string _dataFile = "../automobileParts.json";
    private static Dictionary<string, List<int>> _shopCarts = new();

    private List<AutomobilePart> LoadParts()
    {
        var json = System.IO.File.ReadAllText(_dataFile);
        return JsonSerializer.Deserialize<List<AutomobilePart>>(json) ?? new List<AutomobilePart>();
    }

    [HttpGet("parts")]
    public IActionResult GetParts([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var parts = LoadParts();
        var paged = parts.Skip(offset).Take(limit).ToList();
        return Ok(paged);
    }

    [HttpGet("parts/{id}")]
    public IActionResult GetPartById(int id)
    {
        var part = LoadParts().FirstOrDefault(p => p.Id == id);
        if (part == null) return NotFound();
        return Ok(part);
    }

    [HttpGet("search")]
    public IActionResult SearchParts([FromQuery] string? name, [FromQuery] string? description, [FromQuery] string? manufacturer, [FromQuery] decimal? price)
    {
        var parts = LoadParts().AsQueryable();
        if (!string.IsNullOrEmpty(name))
            parts = parts.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(description))
            parts = parts.Where(p => p.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(manufacturer))
            parts = parts.Where(p => p.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase));
        if (price.HasValue)
            parts = parts.Where(p => p.Price == price.Value);
        return Ok(parts.ToList());
    }

    [HttpPost("cart")]
    public IActionResult CreateCart([FromQuery] string cartId)
    {
        if (_shopCarts.ContainsKey(cartId))
            return BadRequest("Cart already exists.");
        _shopCarts[cartId] = new List<int>();
        return Ok(new { cartId });
    }

    [HttpPost("cart/{cartId}/add")]
    public IActionResult AddToCart(string cartId, [FromQuery] int partId)
    {
        if (!_shopCarts.ContainsKey(cartId))
            return NotFound("Cart not found.");
        var parts = LoadParts();
        if (!parts.Any(p => p.Id == partId))
            return NotFound("Part not found.");
        _shopCarts[cartId].Add(partId);
        return Ok(_shopCarts[cartId]);
    }

    [HttpPost("cart/{cartId}/remove")]
    public IActionResult RemoveFromCart(string cartId, [FromQuery] int partId)
    {
        if (!_shopCarts.ContainsKey(cartId))
            return NotFound("Cart not found.");
        _shopCarts[cartId].Remove(partId);
        return Ok(_shopCarts[cartId]);
    }

    [HttpGet("cart/{cartId}/total")]
    public IActionResult GetCartTotal(string cartId)
    {
        if (!_shopCarts.ContainsKey(cartId))
            return NotFound("Cart not found.");
        var parts = LoadParts();
        var total = _shopCarts[cartId].Select(id => parts.FirstOrDefault(p => p.Id == id)?.Price ?? 0).Sum();
        return Ok(new { total });
    }
}

public class AutomobilePart
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = string.Empty;
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
