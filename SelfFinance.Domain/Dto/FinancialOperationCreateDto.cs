using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class FinancialOperationCreateDto
{
    [Required] public bool IsIncome { get; set; }
    [Required, Range(0, double.MaxValue)] public decimal Sum { get; set; }
    [Required] public int TagId { get; set; }
}