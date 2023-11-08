using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SelfFinance.Data;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Domain.Services;

public class FinancialOperationService : IFinancialOperationService
{
    private readonly SelfFinanceDbContext _context;

    public FinancialOperationService(SelfFinanceDbContext context)
    {
        _context = context;
    }

    public (decimal totalIncomes, decimal totalExpenses) CalculateTotal(IEnumerable<FinancialOperation> operations)
    {
        var totalIncome = 0m;
        var totalExpenses = 0m;

        foreach (var operation in operations)
        {
            if (operation.IsIncome)
            {
                totalIncome += operation.Sum;
            }
            else
            {
                totalExpenses += operation.Sum;
            }
        }

        return (totalIncome, totalExpenses);
    }

    public async Task<FinancialOperation> GetAsync(int id)
    {
        return await _context.FinancialOperations
                   .Where(fo => !fo.IsDeleted)
                   .SingleOrDefaultAsync(fo => fo.Id == id)
               ?? throw new EntityNotFoundException();
    }

    public async Task<IEnumerable<FinancialOperation>> GetAllAsync()
    {
        return await _context.FinancialOperations
            .Where(fo => !fo.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialOperation>> GetAllAsync(DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new ArgumentException("From should precede to");
        }

        return await _context.FinancialOperations
            .Where(fo =>
                !fo.IsDeleted &&
                fo.CreatedOn >= from &&
                fo.CreatedOn <= to)
            .ToListAsync();
    }

    public async Task<int> AddAsync(FinancialOperationCreateDto dto)
    {
        var createdOn = DateTime.Now;

        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        var incomeTagId = dto.IsIncome ? dto.TagId : null;
        var expenseTagId = dto.IsIncome ? null : dto.TagId;

        var incomeTag = incomeTagId is not null
            ? await _context.IncomeTags.SingleOrDefaultAsync(tag =>
                !tag.IsDeleted &&
                tag.Id == incomeTagId) ?? throw new EntityNotFoundException("Income tag not found")
            : null;

        var expenseTag = expenseTagId is not null
            ? await _context.ExpenseTags.SingleOrDefaultAsync(tag =>
                !tag.IsDeleted &&
                tag.Id == expenseTagId) ?? throw new EntityNotFoundException("Expense tag not found")
            : null;

        var operation = new FinancialOperation
        {
            IsIncome = dto.IsIncome,
            Sum = dto.Sum,
            IncomeTagId = incomeTagId,
            IncomeTag = incomeTag,
            ExpenseTagId = expenseTagId,
            ExpenseTag = expenseTag,
            CreatedOn = createdOn
        };

        await _context.FinancialOperations.AddAsync(operation);
        await _context.SaveChangesAsync();

        return operation.Id;
    }

    public async Task UpdateAsync(FinancialOperationUpdateDto dto)
    {
        var updatedOn = DateTime.Now;

        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        var operation = await _context.FinancialOperations
                            .SingleOrDefaultAsync(op =>
                                !op.IsDeleted &&
                                op.Id == dto.Id)
                        ?? throw new EntityNotFoundException("Operation not found");

        if (operation.IsIncome)
        {
            var incomeTag = dto.TagId is not null
                ? await _context.IncomeTags.SingleOrDefaultAsync(tag =>
                    !tag.IsDeleted &&
                    tag.Id == dto.TagId) ?? throw new EntityNotFoundException("Income tag not found")
                : null;

            operation.IncomeTagId = dto.TagId;
            operation.IncomeTag = incomeTag;
        }
        else
        {
            var expenseTag = dto.TagId is not null
                ? await _context.ExpenseTags.SingleOrDefaultAsync(tag =>
                    !tag.IsDeleted &&
                    tag.Id == dto.TagId) ?? throw new EntityNotFoundException("Expense tag not found")
                : null;

            operation.ExpenseTagId = dto.TagId;
            operation.ExpenseTag = expenseTag;
        }

        operation.UpdatedOn = updatedOn;

        _context.Update(operation);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var deletedOn = DateTime.Now;

        var operation = await _context.FinancialOperations
                            .SingleOrDefaultAsync(op =>
                                !op.IsDeleted &&
                                op.Id == id)
                        ?? throw new EntityNotFoundException("Operation not found");

        operation.IsDeleted = true;
        operation.DeletedOn = deletedOn;

        _context.Update(operation);
        await _context.SaveChangesAsync();
    }
}