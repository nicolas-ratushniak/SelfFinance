using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SelfFinance.Api.Helpers;
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
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var operations = await _financialOperationService.GetAllAsync();
        return Ok(operations);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        try
        {
            var operation = await _financialOperationService.GetAsync(id);
            return Ok(operation.ConvertToDto());
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] FinancialOperationCreateDto dto)
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
    public async Task<IActionResult> UpdateAsync([FromBody] FinancialOperationUpdateDto dto)
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
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
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
}