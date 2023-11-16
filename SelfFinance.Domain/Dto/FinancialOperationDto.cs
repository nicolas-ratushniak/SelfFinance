namespace SelfFinance.Domain.Dto;

public class FinancialOperationDto
{
    public int Id { get; set; }
    public decimal Sum { get; set; }
    public DateTime OperationDate { get; set; }
    public int OperationTagId { get; set; }
}