using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SelfFinance.Data;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Domain.Services;

public class ExpenseTagService : IExpenseTagService
{
    private readonly SelfFinanceDbContext _context;

    public ExpenseTagService(SelfFinanceDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseTag> GetAsync(int id)
    {
        return await _context.ExpenseTags
            .SingleOrDefaultAsync(tag =>
                !tag.IsDeleted &&
                tag.Id == id) ?? throw new EntityNotFoundException();
    }

    public async Task<IEnumerable<ExpenseTag>> GetAllAsync()
    {
        return await _context.ExpenseTags
            .Where(tag => !tag.IsDeleted)
            .ToListAsync();
    }

    public async Task<int> AddAsync(ExpenseTagCreateDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        if (await _context.ExpenseTags.AnyAsync(tag => tag.Name == dto.Name))
        {
            throw new ValidationException("The name is already in use");
        }

        var tag = new ExpenseTag
        {
            Name = dto.Name
        };

        await _context.AddAsync(tag);
        await _context.SaveChangesAsync();
        
        return tag.Id;
    }

    public async Task UpdateAsync(ExpenseTagUpdateDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        if (await _context.ExpenseTags.AnyAsync(tag => 
                tag.Name == dto.Name &&
                tag.Id != dto.Id))
        {
            throw new ValidationException("The name is already in use");
        }

        var tag = await GetAsync(dto.Id);

        tag.Name = dto.Name;

        _context.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var deletedOn = DateTime.Now;
        
        var tag = await GetAsync(id);

        tag.DeletedOn = deletedOn;
        tag.IsDeleted = true;

        _context.Update(tag);
        await _context.SaveChangesAsync();
    }
}