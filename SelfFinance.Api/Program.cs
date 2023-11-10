using Microsoft.EntityFrameworkCore;
using SelfFinance.Data;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<SelfFinanceDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddScoped<IExpenseTagService, ExpenseTagService>();
builder.Services.AddScoped<IIncomeTagService, IncomeTagService>();
builder.Services.AddScoped<IFinancialOperationService, FinancialOperationService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();