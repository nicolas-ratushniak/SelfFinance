using System.ComponentModel.DataAnnotations;

namespace SelfFinance.Domain.Dto;

public class FinancialOperationUpdateDto
{
    [Required] public int Id { get; set; }
    [Required] public int OperationTagId { get; set; }
}