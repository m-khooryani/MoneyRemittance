using Microsoft.EntityFrameworkCore;
using MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore;

namespace MoneyRemittance.Infrastructure;

public class MoneyRemittanceDbContext : ServiceDbContext
{
    public MoneyRemittanceDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
