namespace SelfFinance.Client.ViewModels;

public class ReportViewModel
{
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Total => Income - Expense;
    public IEnumerable<TransactionViewModel> Transactions { get; set; }
}