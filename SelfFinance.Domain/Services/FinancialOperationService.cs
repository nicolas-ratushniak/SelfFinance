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

    public async Task<FinancialOperation> GetAsync(int id)
    {
        return await _context.FinancialOperations
                   .Where(fo => !fo.IsDeleted)
                   .SingleOrDefaultAsync(fo => fo.Id == id)
               ?? throw new EntityNotFoundException();
    }

    public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync()
    {
        return await _context.FinancialOperations
            .Where(fo => !fo.IsDeleted)
            .Select(fo => new FinancialOperationDto
            {
                Id = fo.Id,
                Sum = fo.Sum,
                OperationDate = fo.OperationDate,
                OperationTagId = fo.OperationTagId
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync(DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new ArgumentException("From should precede To");
        }

        return await _context.FinancialOperations
            .Where(fo =>
                !fo.IsDeleted &&
                fo.OperationDate >= from &&
                fo.OperationDate <= to)
            .Select(fo => new FinancialOperationDto
                {
                    Id = fo.Id,
                    Sum = fo.Sum,
                    OperationDate = fo.OperationDate,
                    OperationTagId = fo.OperationTagId
                })
            .ToListAsync();
    }

    public async Task<int> AddAsync(FinancialOperationCreateDto dto)
    {
        var createdOn = DateTime.Now;

        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        if (dto.OperationDate > DateTime.Now)
        {
            throw new ValidationException("Future date unsupported");
        }

        var operationTag = _context.OperationTags
                               .SingleOrDefault(t => !t.IsDeleted && t.Id == dto.OperationTagId)
                           ?? throw new EntityNotFoundException("Operation tag not found");

        var operation = new FinancialOperation
        {
            Sum = dto.Sum,
            OperationTagId = dto.OperationTagId,
            OperationTag = operationTag,
            OperationDate = dto.OperationDate,
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

        var operationTag = _context.OperationTags
                               .SingleOrDefault(t => !t.IsDeleted && t.Id == dto.OperationTagId)
                           ?? throw new EntityNotFoundException("Operation tag not found");

        operation.Sum = dto.Sum;
        operation.OperationDate = dto.OperationDate;
        operation.OperationTagId = dto.OperationTagId;
        operation.OperationTag = operationTag;
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