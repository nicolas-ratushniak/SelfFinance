using Newtonsoft.Json;
using SelfFinance.Client.Abstract;
using SelfFinance.Client.Helpers;
using SelfFinance.Client.ViewModels;
using SelfFinance.Domain.Dto;

namespace SelfFinance.Client.Services;

public class ReportService : IReportService
{
    private readonly HttpClient _client;

    public ReportService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ReportViewModel> GetDailyReportAsync(DateOnly date)
    {
        var dateString = date.ToString("yyyy-MM-dd");

        using var reportResponse = await _client.GetAsync($"reports/daily-report?date={dateString}");
        reportResponse.EnsureSuccessStatusCode();

        var reportJsonResult = await reportResponse.Content.ReadAsStringAsync();
        var reportDto = JsonConvert.DeserializeObject<ReportDto>(reportJsonResult)!;

        return reportDto.ConvertToViewModel();
    }

    public async Task<ReportViewModel> GetPeriodReportAsync(DateOnly startDate, DateOnly endDate)
    {
        var startDateString = startDate.ToString("yyyy-MM-dd");
        var endDateString = endDate.ToString("yyyy-MM-dd");

        using var reportResponse = await _client.GetAsync(
            $"reports/period-report?startDate={startDateString}&endDate={endDateString}");

        reportResponse.EnsureSuccessStatusCode();

        var reportJsonResult = await reportResponse.Content.ReadAsStringAsync();
        var reportDto = JsonConvert.DeserializeObject<ReportDto>(reportJsonResult)!;

        return reportDto.ConvertToViewModel();
    }
}