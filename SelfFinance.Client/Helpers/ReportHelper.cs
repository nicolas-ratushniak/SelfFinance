using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Helpers;

public static class ReportHelper
{
    public static ReportViewModel ConvertToViewModel(this ReportDto dto, IEnumerable<OperationTagDto> tagDtos)
    {
        return new ReportViewModel
        {
            TotalIncome = dto.TotalIncome,
            TotalExpense = dto.TotalExpense,
            Transactions = dto.Transactions.ConvertToViewModels(tagDtos)
        };
    }
}