namespace SelfFinance.Api.Dto;

public class FinancialOperationDto
{
    public int Id { get; set; }
    public bool IsIncome { get; set; }
    public decimal Sum { get; set; }
    public int TagId { get; set; }
}