using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Blazor.Models;

public class DailyReport
{
    [Required] public DateOnly Date { get; set; }
}