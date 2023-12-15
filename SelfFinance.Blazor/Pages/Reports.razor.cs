using Microsoft.AspNetCore.Components;
using SelfFinance.Blazor.Components.Abstract;
using SelfFinance.Blazor.Components.Modals;
using SelfFinance.Blazor.Components.Tables;
using SelfFinance.Blazor.Models;
using SelfFinance.Client.Abstract;
using SelfFinance.Client.ViewModels;

namespace SelfFinance.Blazor.Pages;

public partial class Reports
{
    private Popup _popup;
    private TransactionsReadOnlyTable _transactionsTable;

    private decimal Total { get; set; }
    private decimal Income { get; set; }
    private decimal Expense { get; set; }
    public IEnumerable<TransactionViewModel>? Transactions { get; set; }

    [Inject] private IReportService ReportService { get; set; }
 
    private async Task GenerateDailyReportAsync(DailyReport reportInfo)
    {
        try
        {
            var report = await ReportService.GetDailyReportAsync(reportInfo.Date);

            Total = report.Total;
            Income = report.Income;
            Expense = report.Expense;
            Transactions = report.Transactions;
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            _popup.PopupAsync(PopupType.Error, "Bad request", "A server error occured");
        }
    }
    
    private async Task GeneratePeriodReportAsync(PeriodReport reportInfo)
    {
        try
        {
            var report = await ReportService.GetPeriodReportAsync(reportInfo.StartDate, reportInfo.EndDate);

            Total = report.Total;
            Income = report.Income;
            Expense = report.Expense;
            Transactions = report.Transactions;
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            _popup.PopupAsync(PopupType.Error, "Bad request", "A server error occured");
        }
    }
}