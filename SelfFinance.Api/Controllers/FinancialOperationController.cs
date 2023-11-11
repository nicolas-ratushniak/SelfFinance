using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SelfFinance.Api.Dto;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/transactions")]
public class FinancialOperationController : ControllerBase
{
    private readonly IFinancialOperationService _financialOperationService;
    private readonly ILogger<FinancialOperationController> _logger;

    public FinancialOperationController(
        IFinancialOperationService financialOperationService,
        ILogger<FinancialOperationController> logger)
    {
        _financialOperationService = financialOperationService;
        _logger = logger;
    }

    [HttpGet("daily-report")]
    public async Task<JsonResult> GetDailyReport([FromQuery] DateOnly date)
    {
        return await GetDatePeriodReport(date, date);
    }
    
    [HttpGet("report")]
    public async Task<JsonResult> GetDatePeriodReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        var from = startDate.ToDateTime(TimeOnly.MinValue);
        var to = endDate.ToDateTime(TimeOnly.MaxValue);
        
        var operations = (await _financialOperationService.GetAllAsync(from, to)).ToList();
        var totals = _financialOperationService.CalculateTotal(operations);

        var report = new ReportDto
        {
            TotalIncome = totals.totalIncomes,
            TotalExpense = totals.totalExpenses,
            Transactions = operations.Select(OperationToDto).ToList()
        };

        return new JsonResult(report);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllOperationsAsync()
    {
        var operations = (await _financialOperationService.GetAllAsync())
            .Select(OperationToDto);

        return new ObjectResult(operations);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOperationAsync(int id)
    {
        try
        {
            var operation = await _financialOperationService.GetAsync(id);

            return new ObjectResult(OperationToDto(operation));
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddOperationAsync([FromBody] FinancialOperationCreateDto dto)
    {
        try
        {
            await _financialOperationService.AddAsync(dto);
            return Ok();
        }
        catch (ValidationException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Validation error occured: {ex.Message}");
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateOperationAsync([FromBody] FinancialOperationUpdateDto dto)
    {
        try
        {
            await _financialOperationService.UpdateAsync(dto);
            return Ok();
        }
        catch (ValidationException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Validation error occured: {ex.Message}");
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOperationAsync(int id)
    {
        try
        {
            await _financialOperationService.SoftDeleteAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }

    private FinancialOperationDto OperationToDto(FinancialOperation operation)
    {
        return new FinancialOperationDto
        {
            Id = operation.Id,
            IsIncome = operation.IsIncome,
            Sum = operation.Sum,
            TagId = operation.IsIncome ? (int)operation.IncomeTagId! : (int)operation.ExpenseTagId!
        };
    }
}