namespace SelfFinance.Client.ViewModels;

public class ReportViewModel
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public IEnumerable<TransactionViewModel> Transactions { get; set; }
}