﻿namespace MoneyRemittance.BuildingBlocks.Domain;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
