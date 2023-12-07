using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SelfFinance.Api.Helpers;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(
        ITransactionService transactionService,
        ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var operations = await _transactionService.GetAllAsync();
        return Ok(operations);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        try
        {
            var operation = await _transactionService.GetAsync(id);
            return Ok(operation.ConvertToDto());
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] TransactionCreateDto dto)
    {
        try
        {
            var id = await _transactionService.AddAsync(dto);
            return Ok(id);
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
    public async Task<IActionResult> UpdateAsync([FromBody] TransactionUpdateDto dto)
    {
        try
        {
            await _transactionService.UpdateAsync(dto);
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
            await _transactionService.SoftDeleteAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
}