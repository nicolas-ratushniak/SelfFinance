namespace SelfFinance.Api.Dto;

public class ReportDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public List<FinancialOperationDto> Transactions { get; set; }
}