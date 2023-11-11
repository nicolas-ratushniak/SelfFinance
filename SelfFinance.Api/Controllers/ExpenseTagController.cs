using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SelfFinance.Api.Dto;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/expense-tags")]
public class ExpenseTagController : ControllerBase
{
    private readonly IExpenseTagService _expenseTagService;
    private readonly ILogger<ExpenseTagController> _logger;

    public ExpenseTagController(IExpenseTagService expenseTagService, ILogger<ExpenseTagController> logger)
    {
        _expenseTagService = expenseTagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenseTagsAsync()
    {
        var tags = (await _expenseTagService.GetAllAsync())
            .Select(tag => new ExpenseTagDto
            {
                Id = tag.Id,
                Name = tag.Name
            });

        return new ObjectResult(tags);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExpenseTagAsync(int id)
    {
        try
        {
            var tag = await _expenseTagService.GetAsync(id);

            return new ObjectResult(new ExpenseTagDto
            {
                Id = tag.Id,
                Name = tag.Name
            });
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddExpenseTagAsync([FromBody] ExpenseTagCreateDto dto)
    {
        try
        {
            await _expenseTagService.AddAsync(dto);
            return Ok();
        }
        catch (ValidationException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Validation error occured: {ex.Message}");
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateExpenseTagAsync([FromBody] ExpenseTagUpdateDto dto)
    {
        try
        {
            await _expenseTagService.UpdateAsync(dto);
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
    public async Task<IActionResult> DeleteExpenseTagAsync(int id)
    {
        try
        {
            await _expenseTagService.SoftDeleteAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
}