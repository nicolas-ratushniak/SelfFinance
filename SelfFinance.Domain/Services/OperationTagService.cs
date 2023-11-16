using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SelfFinance.Data;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Domain.Services;

public class OperationTagService : IOperationTagService
{
    private readonly SelfFinanceDbContext _context;

    public OperationTagService(SelfFinanceDbContext context)
    {
        _context = context;
    }

    public async Task<OperationTag> GetAsync(int id)
    {
        return await _context.OperationTags
            .SingleOrDefaultAsync(tag =>
                !tag.IsDeleted &&
                tag.Id == id) ?? throw new EntityNotFoundException();
    }

    public async Task<IEnumerable<OperationTagDto>> GetAllAsync()
    {
        return await _context.OperationTags
            .Where(tag => !tag.IsDeleted)
            .Select(t => new OperationTagDto
            {
                Id = t.Id,
                OperationType = t.OperationType,
                Name = t.Name
            })
            .ToListAsync();
    }

    public async Task<int> AddAsync(OperationTagCreateDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        if (await _context.OperationTags.AnyAsync(tag => tag.Name == dto.Name))
        {
            throw new ValidationException("The name is already in use");
        }

        var tag = new OperationTag
        {
            Name = dto.Name,
            OperationType = dto.OperationType
        };

        await _context.AddAsync(tag);
        await _context.SaveChangesAsync();
        
        return tag.Id;
    }

    public async Task UpdateAsync(OperationTagUpdateDto dto)
    {
        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        if (await _context.OperationTags.AnyAsync(tag => 
                tag.Name == dto.Name &&
                tag.Id != dto.Id))
        {
            throw new ValidationException("The name is already in use");
        }

        var tag = await GetAsync(dto.Id);

        tag.Name = dto.Name;
        tag.OperationType = dto.OperationType;

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