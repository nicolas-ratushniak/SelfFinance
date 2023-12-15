using SelfFinance.Client.ViewModels;

namespace SelfFinance.Client.Abstract;

public interface IReportService
{
    public Task<ReportViewModel> GetDailyReportAsync(DateOnly date);
    public Task<ReportViewModel> GetPeriodReportAsync(DateOnly startDate, DateOnly endDate);
}