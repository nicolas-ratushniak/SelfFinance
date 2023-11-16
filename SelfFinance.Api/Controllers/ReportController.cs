using Microsoft.AspNetCore.Mvc;
using SelfFinance.Api.Dto;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/reports")]
public class ReportController : Controller
{
    private readonly IFinancialOperationService _financialOperationService;

    public ReportController(IFinancialOperationService financialOperationService)
    {
        _financialOperationService = financialOperationService;
    }

    [HttpGet("daily-report")]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateOnly date)
    {
        return await GetDatePeriodReport(date, date);
    }

    [HttpGet("period-report")]
    public async Task<IActionResult> GetDatePeriodReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var from = startDate.ToDateTime(TimeOnly.MinValue);
        var to = endDate.ToDateTime(TimeOnly.MaxValue);

        var operations = (await _financialOperationService.GetAllAsync(from, to)).ToList();
        var totals = await _financialOperationService.CalculateTotalAsync(operations);

        var report = new ReportDto
        {
            TotalIncome = totals[OperationType.Income],
            TotalExpense = totals[OperationType.Expense],
            Transactions = operations
        };

        return Ok(report);
    }
}