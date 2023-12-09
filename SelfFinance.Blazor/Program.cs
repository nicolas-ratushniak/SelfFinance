using SelfFinance.Client.Abstract;
using SelfFinance.Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var baseAddress = new Uri("https://localhost:7242/self-finance/api/");

builder.Services.AddHttpClient<ITransactionService, TransactionService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IOperationTagService, OperationTagService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IReportService, ReportService>(client => client.BaseAddress = baseAddress);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();