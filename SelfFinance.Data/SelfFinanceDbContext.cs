using Microsoft.EntityFrameworkCore;
using SelfFinance.Data.Models;

namespace SelfFinance.Data;

public class SelfFinanceDbContext : DbContext
{
    public DbSet<FinancialOperation> FinancialOperations { get; set; }
    public DbSet<IncomeTag> IncomeTags { get; set; }
    public DbSet<ExpenseTag> ExpenseTags { get; set; }

    public SelfFinanceDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialOperation>()
            .Property(f => f.Sum)
            .HasPrecision(2);

        base.OnModelCreating(modelBuilder);
    }
}