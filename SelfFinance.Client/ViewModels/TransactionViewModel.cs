namespace SelfFinance.Client.ViewModels;

public class TransactionViewModel
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string TagName { get; set; }
    public decimal SignedSum { get; set; }
    public decimal AbsoluteSum => Math.Abs(SignedSum);
}