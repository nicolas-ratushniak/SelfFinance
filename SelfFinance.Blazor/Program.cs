using SelfFinance.Client.Abstract;
using SelfFinance.Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient<ITransactionService, TransactionService>(client => 
    client.BaseAddress = new Uri("http://localhost:5149/self-finance/api/"));

builder.Services.AddHttpClient<IOperationTagService, OperationTagService>(client => 
    client.BaseAddress = new Uri("http://localhost:5149/self-finance/api/"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();