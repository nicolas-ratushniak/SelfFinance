using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SelfFinance.Data;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<SelfFinanceDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("self-finance-v1", new OpenApiInfo
    {
        Title = "Self Finance API",
        Description = "Manage your money flow", Version = "v1"
    }));

builder.Services.AddScoped<IExpenseTagService, ExpenseTagService>();
builder.Services.AddScoped<IIncomeTagService, IncomeTagService>();
builder.Services.AddScoped<IFinancialOperationService, FinancialOperationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/self-finance-v1/swagger.json", "Self Finance API V1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/IncomeTags/", 
    async (IIncomeTagService service) => await service.GetAllAsync());
app.MapGet("/IncomeTags/{id:int}", 
    async (IIncomeTagService service, int id) => await service.GetAsync(id));
app.MapPost("/IncomeTags/", 
    async (IIncomeTagService service, [FromBody] IncomeTagCreateDto dto) => await service.AddAsync(dto));
app.MapPut("/IncomeTags/", 
    async (IIncomeTagService service, [FromBody] IncomeTagUpdateDto dto) => await service.UpdateAsync(dto));
app.MapDelete("/IncomeTags/{id:int}", 
    async (IIncomeTagService service, int id) => await service.SoftDeleteAsync(id));

app.Run();