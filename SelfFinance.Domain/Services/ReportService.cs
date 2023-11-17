using Microsoft.EntityFrameworkCore;
using SelfFinance.Data;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Services;

public class ReportService : IReportService
{
    private readonly SelfFinanceDbContext _context;
    private readonly IFinancialOperationService _financialOperationService;

    public ReportService(SelfFinanceDbContext context, IFinancialOperationService financialOperationService)
    {
        _context = context;
        _financialOperationService = financialOperationService;
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

        var operations = (await _financialOperationService.GetAllAsync(from, to)).ToList();
        var totals = await CalculateTotalAsync(operations);

        return new ReportDto
        {
            TotalIncome = totals[OperationType.Income],
            TotalExpense = totals[OperationType.Expense],
            Transactions = operations
        };
    }

    private async Task<Dictionary<OperationType, decimal>> CalculateTotalAsync(
        IEnumerable<FinancialOperationDto> operations)
    {
        var operationTags = await _context.OperationTags.ToListAsync();

        var resultDict = operations
            .GroupBy(o => operationTags
                .Single(t => t.Id == o.OperationTagId).OperationType)
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