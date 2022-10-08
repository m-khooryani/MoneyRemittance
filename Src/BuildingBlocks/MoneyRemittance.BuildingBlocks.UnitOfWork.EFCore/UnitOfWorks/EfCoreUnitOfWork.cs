using MoneyRemittance.BuildingBlocks.Domain;

namespace MoneyRemittance.BuildingBlocks.UnitOfWork.EFCore.UnitOfWorks;

internal class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly ServiceDbContext _context;

    public EfCoreUnitOfWork(
        ServiceDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
