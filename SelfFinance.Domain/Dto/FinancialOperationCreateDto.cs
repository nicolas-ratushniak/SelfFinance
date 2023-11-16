using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class FinancialOperationCreateDto
{
    [Required, Range(1, 1_000_000)] public decimal Sum { get; set; }
    [Required] public DateTime OperationDate { get; set; }
    [Required] public int OperationTagId { get; set; }
}