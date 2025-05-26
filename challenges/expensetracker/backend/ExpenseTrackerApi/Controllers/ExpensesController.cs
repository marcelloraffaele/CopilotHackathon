using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace ExpenseTrackerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private static readonly List<string> Categories = new List<string>
        {
            "Food", "Transport", "Entertainment", "Shopping", "Bills",
            "Health", "Education", "Travel", "Groceries", "Other"
        };

        private static readonly ConcurrentBag<Expense> Expenses = new();
        private static int _idCounter = 1;

        [HttpGet("categories")]
        public ActionResult<IEnumerable<string>> GetCategories() => Categories;

        [HttpPost]
        public ActionResult<Expense> AddExpense([FromBody] ExpenseDto dto)
        {
            if (!Categories.Contains(dto.Category))
                return BadRequest("Invalid category.");
            var expense = new Expense
            {
                Id = _idCounter++,
                Amount = dto.Amount,
                Category = dto.Category,
                Date = dto.Date ?? DateTime.Now,
                Description = dto.Description
            };
            Expenses.Add(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Expense>> GetAll() => Expenses.OrderByDescending(e => e.Date).ToList();

        [HttpGet("{id}")]
        public ActionResult<Expense> GetExpense(int id)
        {
            var expense = Expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null) return NotFound();
            return expense;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExpense(int id)
        {
            var expense = Expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null) return NotFound();
            var removed = Expenses.ToList().Remove(expense);
            if (removed)
            {
                Expenses.Clear();
                foreach (var e in Expenses.ToList().Where(e => e.Id != id))
                    Expenses.Add(e);
                return NoContent();
            }
            return StatusCode(500, "Failed to delete expense.");
        }

        [HttpGet("summary/monthly")]
        public ActionResult<decimal> GetMonthlyTotal()
        {
            var now = DateTime.Now;
            var total = Expenses.Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
                                .Sum(e => e.Amount);
            return total;
        }

        [HttpGet("summary/category")]
        public ActionResult<IEnumerable<CategorySummary>> GetCategoryBreakdown()
        {
            var now = DateTime.Now;
            var breakdown = Expenses.Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
                .GroupBy(e => e.Category)
                .Select(g => new CategorySummary { Category = g.Key, Total = g.Sum(e => e.Amount) })
                .ToList();
            return breakdown;
        }
    }

    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }

    public class ExpenseDto
    {
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
    }

    public class CategorySummary
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
