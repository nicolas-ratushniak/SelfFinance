using Microsoft.AspNetCore.Components;
using SelfFinance.Blazor.Components;
using SelfFinance.Client.Abstract;

namespace SelfFinance.Blazor.Pages;

public partial class Reports
{
    private Popup _popup;
    
    [Inject] private IReportService ReportService { get; set; }
}