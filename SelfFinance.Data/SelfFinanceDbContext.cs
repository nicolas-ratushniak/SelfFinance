﻿using Microsoft.EntityFrameworkCore;
using SelfFinance.Data.Models;

namespace SelfFinance.Data;

public class SelfFinanceDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<OperationTag> OperationTags { get; set; }

    public SelfFinanceDbContext(DbContextOptions options) : base(options)
    {
    }
}