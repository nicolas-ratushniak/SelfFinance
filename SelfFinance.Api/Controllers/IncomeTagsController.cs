using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SelfFinance.Api.Dto;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/income-tags")]
public class IncomeTagsController : ControllerBase
{
    private readonly IIncomeTagService _incomeTagService;
    private readonly ILogger<IncomeTagsController> _logger;

    public IncomeTagsController(IIncomeTagService incomeTagService, ILogger<IncomeTagsController> logger)
    {
        _incomeTagService = incomeTagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIncomeTagsAsync()
    {
        var tags = (await _incomeTagService.GetAllAsync())
            .Select(tag => new IncomeTagDto
            {
                Id = tag.Id,
                Name = tag.Name
            });

        return new ObjectResult(tags);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetIncomeTagAsync(int id)
    {
        try
        {
            var tag = await _incomeTagService.GetAsync(id);

            return new ObjectResult(new IncomeTagDto
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
    public async Task<IActionResult> AddIncomeTagAsync([FromBody] IncomeTagCreateDto dto)
    {
        try
        {
            await _incomeTagService.AddAsync(dto);
            return Ok();
        }
        catch (ValidationException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Validation error occured: {ex.Message}");
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateIncomeTagAsync([FromBody] IncomeTagUpdateDto dto)
    {
        try
        {
            await _incomeTagService.UpdateAsync(dto);
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
    public async Task<IActionResult> DeleteIncomeTagAsync(int id)
    {
        try
        {
            await _incomeTagService.SoftDeleteAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return NotFound();
        }
    }
}