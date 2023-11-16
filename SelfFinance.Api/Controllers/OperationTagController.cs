using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/tags")]
public class OperationTagController : ControllerBase
{
    private readonly IOperationTagService _operationTagService;
    private readonly ILogger<OperationTagController> _logger;

    public OperationTagController(IOperationTagService operationTagService, ILogger<OperationTagController> logger)
    {
        _operationTagService = operationTagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var tags = await _operationTagService.GetAllAsync();

        return Ok(tags);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetExpenseTagAsync([FromRoute] int id)
    {
        try
        {
            var tag = await _operationTagService.GetAsync(id);

            return Ok(new OperationTagDto
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
    public async Task<IActionResult> AddAsync([FromBody] OperationTagCreateDto dto)
    {
        try
        {
            await _operationTagService.AddAsync(dto);
            return Ok();
        }
        catch (ValidationException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Validation error occured: {ex.Message}");
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateExpenseTagAsync([FromBody] OperationTagUpdateDto dto)
    {
        try
        {
            await _operationTagService.UpdateAsync(dto);
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
    public async Task<IActionResult> DeleteExpenseTagAsync([FromRoute] int id)
    {
        try
        {
            await _operationTagService.SoftDeleteAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
}