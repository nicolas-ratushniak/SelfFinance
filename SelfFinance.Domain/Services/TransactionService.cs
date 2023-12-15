using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SelfFinance.Data;
using SelfFinance.Data.Models;
using SelfFinance.Domain.Abstract;
using SelfFinance.Domain.Dto;
using SelfFinance.Domain.Exceptions;

namespace SelfFinance.Domain.Services;

public class TransactionService : ITransactionService
{
    private readonly SelfFinanceDbContext _context;

    public TransactionService(SelfFinanceDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> GetAsync(int id)
    {
        return await _context.Transactions
                   .Where(t => !t.IsDeleted)
                   .SingleOrDefaultAsync(t => t.Id == id)
               ?? throw new EntityNotFoundException();
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync()
    {
        return await _context.Transactions
            .Where(t => !t.IsDeleted)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Sum = t.Sum,
                OperationDate = t.OperationDate,
                OperationTagId = t.OperationTagId
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync(DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new ArgumentException("From should precede To");
        }

        return await _context.Transactions
            .Where(t =>
                !t.IsDeleted &&
                t.OperationDate >= from &&
                t.OperationDate <= to)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Sum = t.Sum,
                OperationDate = t.OperationDate,
                OperationTagId = t.OperationTagId
            })
            .ToListAsync();
    }

    public async Task<TransactionRichDto> GetRichAsync(int id)
    {
        var transaction = await _context.Transactions
                              .Include(t => t.OperationTag)
                              .Where(t => !t.IsDeleted)
                              .SingleOrDefaultAsync(t => t.Id == id)
                          ?? throw new EntityNotFoundException();

        return new TransactionRichDto
        {
            Id = transaction.Id,
            Sum = transaction.Sum,
            OperationDate = transaction.OperationDate,
            OperationTag = new OperationTagDto
            {
                Id = transaction.OperationTagId,
                OperationType = transaction.OperationTag.OperationType,
                Name = transaction.OperationTag.Name
            }
        };
    }

    public async Task<IEnumerable<TransactionRichDto>> GetAllRichAsync()
    {
        return await _context.Transactions
            .Include(t => t.OperationTag)
            .Where(t => !t.IsDeleted)
            .Select(t => new TransactionRichDto
            {
                Id = t.Id,
                Sum = t.Sum,
                OperationDate = t.OperationDate,
                OperationTag = new OperationTagDto
                {
                    Id = t.OperationTagId,
                    OperationType = t.OperationTag.OperationType,
                    Name = t.OperationTag.Name
                }
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TransactionRichDto>> GetAllRichAsync(DateTime from, DateTime to)
    {
        if (from > to)
        {
            throw new ArgumentException("From should precede To");
        }

        return await _context.Transactions
            .Where(t =>
                !t.IsDeleted &&
                t.OperationDate >= from &&
                t.OperationDate <= to)
            .Select(t => new TransactionRichDto
            {
                Id = t.Id,
                Sum = t.Sum,
                OperationDate = t.OperationDate,
                OperationTag = new OperationTagDto
                {
                    Id = t.OperationTagId,
                    OperationType = t.OperationTag.OperationType,
                    Name = t.OperationTag.Name
                }
            })
            .ToListAsync();
    }

    public async Task<int> AddAsync(TransactionCreateDto dto)
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

        var operation = new Transaction
        {
            Sum = dto.Sum,
            OperationTagId = dto.OperationTagId,
            OperationTag = operationTag,
            OperationDate = dto.OperationDate,
            CreatedOn = createdOn
        };

        await _context.Transactions.AddAsync(operation);
        await _context.SaveChangesAsync();

        return operation.Id;
    }

    public async Task UpdateAsync(TransactionUpdateDto dto)
    {
        var updatedOn = DateTime.Now;

        Validator.ValidateObject(dto, new ValidationContext(dto), true);

        var operation = await _context.Transactions
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

        var operation = await _context.Transactions
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