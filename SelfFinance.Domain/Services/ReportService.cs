using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Services;

public class ReportService : IReportService
{
    private readonly ITransactionService _transactionService;

    public ReportService(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    public async Task<ReportDto> GetReportAsync(DateOnly date)
    {
        return await GetReportAsync(date, date);
    }

    public async Task<ReportDto> GetReportAsync(DateOnly startDate, DateOnly endDate)
    {
        var from = startDate.ToDateTime(TimeOnly.MinValue);
        var to = endDate.ToDateTime(TimeOnly.MaxValue);
        
        if (from > to)
        {
            throw new ArgumentException("Start date should precede end date");
        }

        var operations = (await _transactionService.GetAllRichAsync(from, to)).ToList();
        var totals = CalculateTotal(operations);

        return new ReportDto
        {
            TotalIncome = totals[OperationType.Income],
            TotalExpense = totals[OperationType.Expense],
            Transactions = operations
        };
    }

    private Dictionary<OperationType, decimal> CalculateTotal(IEnumerable<TransactionRichDto> operations)
    {
        var resultDict = operations
            .GroupBy(o => o.OperationTag.OperationType)
            .ToDictionary(g => g.Key, 
                g => g.Sum(o => o.Sum));

        // ensure all keys are present
        foreach (var tag in Enum.GetValues<OperationType>())
        {
            resultDict.TryAdd(tag, 0m);
        }

        return resultDict;
    }
}