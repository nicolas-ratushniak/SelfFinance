using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SelfFinance.Data;

public class SelfFinanceDbContextFactory : IDesignTimeDbContextFactory<SelfFinanceDbContext>
{
    public SelfFinanceDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<SelfFinanceDbContext>();
        optionBuilder.UseSqlServer(
            @"Server=NorthernRival\MSSQLSERVER,1433;Database=SelfFinance;Trusted_Connection=True;Encrypt=Optional;");
        
        return new SelfFinanceDbContext(optionBuilder.Options);
    }
}