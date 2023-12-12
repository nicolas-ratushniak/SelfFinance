using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Helpers;

public static class ReportHelper
{
    public static ReportViewModel ConvertToViewModel(this ReportDto dto)
    {
        return new ReportViewModel
        {
            Income = dto.TotalIncome,
            Expense = dto.TotalExpense,
            Transactions = dto.Transactions
                .Select(t => t.ConvertToViewModel())
                .OrderByDescending(t => t.Date)
        };
    }
}