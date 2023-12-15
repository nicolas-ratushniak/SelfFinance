using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Blazor.Models;

public class PeriodReport
{
    [Required] public DateOnly StartDate { get; set; }
    [Required] public DateOnly EndDate { get; set; }
}