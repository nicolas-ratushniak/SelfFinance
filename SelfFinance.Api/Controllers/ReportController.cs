using Microsoft.AspNetCore.Mvc;
using SelfFinance.Domain.Abstract;

namespace SelfFinance.Api.Controllers;

[ApiController]
[Route("self-finance/api/reports")]
public class ReportController : Controller
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportController> _logger;

    public ReportController(IReportService reportService, ILogger<ReportController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("daily-report")]
    public async Task<IActionResult> GetDailyReport([FromQuery] DateOnly date)
    {
        try
        {
            var report = await _reportService.GetReportAsync(date);
            return Ok(report);
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Argument exception: {ex.Message}");
        }
    }

    [HttpGet("period-report")]
    public async Task<IActionResult> GetDatePeriodReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
    {
        try
        {
            var report = await _reportService.GetReportAsync(startDate, endDate);
            return Ok(report);
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation("Handled exception: {Exception}", ex.Message);
            return BadRequest($"Argument exception: {ex.Message}");
        }
    }
}