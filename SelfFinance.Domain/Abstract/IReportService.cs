using SelfFinance.Domain.Dto;

namespace SelfFinance.Domain.Abstract;

public interface IReportService
{
    public Task<ReportDto> GetReportAsync(DateOnly startDate, DateOnly endDate);
    public Task<ReportDto> GetReportAsync(DateOnly date);
}